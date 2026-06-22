# Luồng Hoạt Động Liên Thông Giữa 3 Dịch Vụ Microservices

Tài liệu này mô tả chi tiết luồng hoạt động nghiệp vụ y tế khép kín được phân chia và xử lý bởi 3 dịch vụ độc lập (Microservices) của 3 nhóm phát triển.

---

## 1. Bản Đồ Trách Nhiệm Của Các Nhóm

| Dịch vụ | Vai trò phát triển | Nhiệm vụ chính | Cơ sở dữ liệu |
| :--- | :--- | :--- | :--- |
| **Appointment Service** (Nhóm 5) | Quản lý đặt lịch và phân luồng | - Quản lý hồ sơ bác sĩ, lịch trực, chuyên khoa, phí khám.<br>- Đặt lịch khám phía Bệnh nhân (chọn Chuyên khoa -> Bác sĩ -> Giờ trống).<br>- Phát hiện trùng giờ khám (Conflict slot).<br>- Tiếp tân duyệt lịch hẹn và cấp Số thứ tự (STT) vào Hàng chờ (Queue). | `AppointmentDB` |
| **Medical Record Service** (Nhóm 4) | Quản lý khám bệnh và hồ sơ | - Quản lý thông tin hành chính, tiền sử bệnh án, dị ứng của Bệnh nhân.<br>- Bác sĩ ghi nhận triệu chứng, chẩn đoán bệnh án.<br>- Kê đơn thuốc từ danh mục thuốc liên thông.<br>- Phát đi sự kiện (Publish event) `prescription.created` sau khi hoàn thành. | `MedicalDB` |
| **Pharmacy & Billing Service** (Nhóm 6) | Quản lý thanh toán và kho thuốc | - Xác thực tập trung (JWT login), quản lý tài khoản cho cả 4 vai trò.<br>- Quản lý kho thuốc (Nhập thuốc, cập nhật giá, tồn kho).<br>- Nhận sự kiện (Consume event) `prescription.created` để tự động trừ kho thuốc.<br>- Tính toán và thu viện phí (Phí khám + Tiền thuốc). | `PharmacyDB` |

---

## 2. Sơ Đồ Quy Trình Hoạt Động Liên Thông (Sequence Diagram)

Sơ đồ dưới đây thể hiện cách các dịch vụ tương tác với nhau từ lúc Bệnh nhân bắt đầu đặt lịch cho đến khi ra về:

```mermaid
sequenceDiagram
    autonumber
    actor BN as Bệnh Nhân
    actor TT as Tiếp Tân
    actor BS as Bác Sĩ
    participant AP as Appointment Service (N5)
    participant MR as Medical Record Service (N4)
    participant PB as Pharmacy & Billing Service (N6)
    participant MQ as Message Broker (RabbitMQ)

    %% Giai đoạn 1: Đặt lịch và kiểm tra trùng slot
    Note over BN, AP: GIAI ĐOẠN 1: ĐẶT LỊCH HẸN KHÁM
    BN->>AP: 1. Xem danh sách Bác sĩ theo Chuyên khoa
    AP-->>BN: Trả về danh sách bác sĩ & lịch trực tương ứng
    BN->>AP: 2. Gửi yêu cầu Đặt lịch (Bác sĩ, Ngày, Khung giờ mong muốn)
    Note over AP: Kiểm tra Conflict Slot:<br/>Khung giờ đã có ai đặt chưa?
    alt Có xung đột giờ khám (Conflict slot)
        AP-->>BN: Trả về mã lỗi 409 (Khung giờ này đã bị trùng)
    else Khung giờ hợp lệ (Không trùng)
        AP->>AP: Lưu lịch hẹn ở trạng thái Chờ xác nhận (Pending)
        AP-->>BN: Thông báo đặt lịch thành công (Chờ tiếp tân duyệt)
    end

    %% Giai đoạn 2: Tiếp tân xác nhận và đẩy vào hàng chờ tivi
    Note over TT, AP: GIAI ĐOẠN 2: TIẾP NHẬN TẠI QUẦY
    TT->>AP: 3. Lấy danh sách lịch hẹn cần duyệt (Pending)
    AP-->>TT: Trả về danh sách
    TT->>AP: 4. Xác nhận bệnh nhân đã đến (Confirm Appointment)
    AP->>AP: Chuyển trạng thái lịch hẹn thành Đã xác nhận (Confirmed)
    AP->>AP: Thêm bệnh nhân vào Hàng chờ khám (Queue) & Cấp Số thứ tự (STT)
    AP-->>TT: Trả về Số thứ tự hàng chờ
    Note over AP: Hiển thị STT lên màn hình Tivi phòng khám

    %% Giai đoạn 3: Bác sĩ khám bệnh và kê đơn
    Note over BS, MR: GIAI ĐOẠN 3: KHÁM BỆNH & KÊ ĐƠN
    BS->>AP: 5. Gọi bệnh nhân tiếp theo trong hàng chờ vào khám
    BS->>MR: 6. Truy xuất hồ sơ bệnh nhân (Thông tin cá nhân, tiền sử bệnh, dị ứng)
    MR-->>BS: Trả về hồ sơ bệnh án cũ
    BS->>MR: 7. Lưu bệnh án mới (Triệu chứng, chẩn đoán, ghi chú)
    BS->>MR: 8. Kê đơn thuốc từ danh mục liên thông
    MR->>MQ: 9. Bắn sự kiện "prescription.created" (Mã bệnh án, mã thuốc, số lượng)
    MR-->>BS: Xác nhận đã lưu bệnh án & đơn thuốc thành công

    %% Giai đoạn 4: Thu tiền và phát thuốc
    Note over PB, MQ: GIAI ĐOẠN 4: THANH TOÁN & PHÁT THUỐC
    MQ->>PB: 10. Consume sự kiện "prescription.created"
    PB->>PB: Tự động trừ tồn kho thuốc tương ứng trong Kho thuốc
    PB->>PB: Tạo hóa đơn y tế (Viện phí = Phí khám bệnh từ N5 + Tiền thuốc kê đơn từ N4)
    BN->>PB: 11. Yêu cầu thanh toán hóa đơn
    PB->>PB: Tiếp nhận tiền thanh toán từ Bệnh nhân
    PB->>PB: Cập nhật trạng thái hóa đơn thành Đã thanh toán (Paid)
    PB-->>BN: 12. Phát thuốc cho bệnh nhân và in hóa đơn ra về
```

---

## 3. Mô Tả Chi Tiết Kịch Bản Nghiệp Vụ

### Bước 1: Đăng nhập hệ thống (Pharmacy & Billing Service - Nhóm 6)
* Người dùng sử dụng tài khoản được quản lý bởi Dịch vụ Nhóm 6 để đăng nhập.
* Dịch vụ Nhóm 6 trả về một **JWT Token** chứa thông tin định danh và vai trò truy cập (`Admin`, `Doctor`, `Receptionist`, `Patient`).
* Token này sẽ được gắn kèm vào tiêu đề (Header) `Authorization: Bearer <token>` trong các yêu cầu gọi API tiếp theo đến API Gateway.

### Bước 2: Đặt lịch và giải quyết xung đột giờ khám (Appointment Service - Nhóm 5)
* Bệnh nhân truy cập ứng dụng, thực hiện chọn Chuyên khoa và Bác sĩ.
* Hệ thống hiển thị các khung giờ làm việc còn trống dựa trên lịch trực của bác sĩ đó.
* Khi bệnh nhân gửi yêu cầu đặt lịch, Dịch vụ Nhóm 5 thực hiện truy vấn cơ sở dữ liệu:
  ```sql
  SELECT COUNT(*) FROM Appointments 
  WHERE DoctorId = @DoctorId 
    AND AppointmentDate = @AppointmentDate 
    AND AppointmentTime = @AppointmentTime
    AND Status != 'Cancelled'
  ```
* **Phát hiện trùng giờ (Conflict slot):**
  * Nếu kết quả trả về `> 0`, hệ thống trả lỗi ngay lập tức về phía giao diện người dùng nhằm ngăn chặn việc hai bệnh nhân cùng đặt chung một khung giờ với cùng một bác sĩ.
  * Nếu kết quả trả về bằng `0`, hệ thống lưu lịch hẹn ở trạng thái `Pending` (Chờ xác nhận).

### Bước 3: Đón tiếp và xếp hàng chờ (Appointment Service - Nhóm 5)
* Khi bệnh nhân đến phòng khám, Tiếp tân kiểm tra thông tin và nhấn **Xác nhận**.
* Lịch hẹn được cập nhật trạng thái từ `Pending` thành `Confirmed`.
* Đồng thời, hệ thống thêm một bản ghi vào bảng hàng chờ của bác sĩ đó:
  ```json
  {
    "appointmentId": 12,
    "doctorId": 3,
    "patientName": "Nguyễn Văn A",
    "queueNumber": 5,
    "status": "Waiting"
  }
  ```
* Màn hình Tivi phòng khám hiển thị số thứ tự y tế liên tục cập nhật theo thời gian thực để bệnh nhân tiện theo dõi.

### Bước 4: Khám bệnh và phát hành sự kiện đơn thuốc (Medical Record Service - Nhóm 4)
* Bác sĩ mở màn hình khám bệnh, xem thông tin hành chính, tiền sử bệnh án cũ, và các thông tin dị ứng từ dữ liệu do Dịch vụ Nhóm 4 quản lý.
* Sau khi thăm khám trực tiếp, bác sĩ nhập dữ liệu bệnh án mới và tiến hành kê đơn thuốc.
* Khi đơn thuốc được lưu thành công, Dịch vụ Nhóm 4 sẽ phát đi sự kiện `prescription.created` đến Message Broker (ví dụ RabbitMQ hoặc Kafka).
  
  **Nội dung Event Message mẫu (`prescription.created`):**
  ```json
  {
    "eventId": "evt_987654321",
    "prescriptionId": 45,
    "appointmentId": 12,
    "patientPhone": "0912345678",
    "doctorFee": 150000.0,
    "medicines": [
      {
        "medicineId": 101,
        "medicineName": "Paracetamol 500mg",
        "quantity": 10,
        "price": 2000.0
      },
      {
        "medicineId": 102,
        "medicineName": "Amoxicillin 500mg",
        "quantity": 20,
        "price": 5000.0
      }
    ],
    "createdAt": "2026-06-22T08:45:00Z"
  }
  ```

### Bước 5: Tiếp nhận đơn thuốc, xuất kho và thu tiền (Pharmacy & Billing Service - Nhóm 6)
* Dịch vụ Nhóm 6 lắng nghe sự kiện `prescription.created` từ hàng đợi chung.
* Ngay khi nhận được thông điệp:
  * **Trừ kho thuốc:** Hệ thống tự động trừ tồn kho của Paracetamol đi 10 đơn vị và Amoxicillin đi 20 đơn vị trong cơ sở dữ liệu `PharmacyDB`.
  * **Tính toán viện phí:** Tạo một hóa đơn (Invoice) cho bệnh nhân.
    $$\text{Tổng viện phí} = \text{Phí khám bệnh (doctorFee)} + \sum (\text{Số lượng thuốc} \times \text{Đơn giá thuốc})$$
    $$\text{Tổng viện phí} = 150.000 + (10 \times 2.000) + (20 \times 5.000) = 270.000 \text{ VNĐ}$$
* Bệnh nhân di chuyển đến quầy thu ngân của Dịch vụ Nhóm 6 để nộp tiền. Sau khi hoàn tất thủ tục thanh toán, trạng thái hóa đơn đổi sang `Paid`, nhân viên phát thuốc và in hóa đơn cho bệnh nhân kết thúc quy trình khám chữa bệnh.
