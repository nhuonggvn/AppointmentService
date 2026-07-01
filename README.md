# HỆ THỐNG ĐẶT LỊCH & QUẢN LÝ PHÒNG KHÁM - CLINICFLOW
## BÀI TẬP LỚN PHÁT TRIỂN ỨNG DỤNG FULL STACK - ĐỀ TÀI 05

Dự án này là mã nguồn tích hợp toàn bộ hệ thống **ClinicFlow** (Đề tài 05 - Hệ thống đặt lịch & quản lý phòng khám) trong môn học Phát triển ứng dụng Full Stack. Hệ thống được phát triển theo kiến trúc **Microservices** phân tán kết hợp với cổng kết nối **API Gateway** và giao diện người dùng **Single Page Application (SPA)** đồng bộ.

Dự án bao gồm thành phần cốt lõi **Appointment Service** (Nhóm 5) cùng với mã nguồn tích hợp của các dịch vụ liên quan từ Nhóm 4 và Nhóm 6 nhằm phục vụ cho quá trình ghép nối, kiểm thử và vận hành liên thông.

---

## 1. Sự khác biệt của nhánh `main` so với các nhánh `master` và `feature`

Nhánh `main` đóng vai trò là **Nhánh tích hợp hệ thống hoàn chỉnh (Integrated System)**. Dưới đây là bảng so sánh chi tiết các thành phần và tính năng giữa các nhánh:

| Thành phần / Tính năng | Nhánh `master` | Nhánh `feature` | Nhánh `main` (Nhánh hiện tại) |
| :--- | :---: | :---: | :---: |
| **Dịch vụ Đặt lịch (Appointment Service - N5)** | Có (Cơ bản) | Có (Cơ bản) | **Có (Hoàn thiện logic, bổ sung Unit Tests)** |
| **Cổng kết nối Gateway (Clinic Gateway - N5)** | Có (Cơ bản) | Có (Cơ bản) | **Có (Cấu hình định tuyến liên thông cho cả 3 nhóm)** |
| **Giao diện Người dùng (Clinic Frontend - N5)** | Không | Có (Chỉ khung xương UI và Mockups tĩnh) | **Có (Đầy đủ logic tích hợp, xử lý phân quyền và cập nhật UI)** |
| **Dịch vụ Bệnh án (Medical Record Service - N4)** | Không | Không | **Có (Tích hợp đầy đủ dự án API Backend Nhóm 4)** |
| **Dịch vụ Kho & Thanh toán (Pharmacy Service - N6)** | Không | Không | **Có (Tích hợp đầy đủ dự án API Backend Nhóm 6)** |
| **Giám sát hệ thống (Health Check API)** | Không | Không | **Có (Tích hợp API `/health` đạt điểm tối đa)** |
| **Chức năng Import file CSV (Admin)** | Không | Không | **Có (Hỗ trợ tải lên danh sách Bác sĩ và Lịch trực)** |
| **Phân trang danh sách (Pagination)** | Không | Không | **Có (Tích hợp phân trang trên giao diện Dashboard)** |
| **Tài liệu hướng dẫn & Scripts phân tích** | Không | Không | **Có (Bổ sung File PDF hướng dẫn và Script Python trích xuất)** |

---

## 2. Bản đồ trách nhiệm và kiến trúc hệ thống

Hệ thống hoạt động dựa trên sự phối hợp của 3 dịch vụ độc lập kết nối qua cổng API Gateway và giao tiếp bất đồng bộ qua Message Broker (RabbitMQ):

| Dịch vụ | Nhóm phát triển | Nhiệm vụ chính | Công nghệ & CSDL |
| :--- | :--- | :--- | :--- |
| **Appointment Service** | Nhóm 5 (Cốt lõi) | - Quản lý hồ sơ bác sĩ, ca trực, chuyên khoa, phí khám.<br>- Đặt lịch khám phía Bệnh nhân (lọc chuyên khoa, bác sĩ, khung giờ trống).<br>- Phát hiện trùng giờ khám (Conflict slot).<br>- Tiếp tân duyệt lịch hẹn và cấp Số thứ tự (STT) vào Hàng chờ (Queue). | ASP.NET Core 9 Web API<br>SQL Server (`AppointmentDB`) |
| **Medical Record Service** | Nhóm 4 | - Quản lý thông tin hành chính, tiền sử bệnh án, dị ứng của Bệnh nhân.<br>- Bác sĩ ghi nhận triệu chứng, chẩn đoán bệnh án.<br>- Kê đơn thuốc từ danh mục thuốc liên thông.<br>- Phát đi sự kiện (Publish event) `prescription.created` sau khi hoàn thành. | ASP.NET Core 8 Web API<br>SQL Server (`MedicalDB`) |
| **Pharmacy & Billing Service** | Nhóm 6 | - Xác thực tập trung (JWT login), quản lý tài khoản cho cả 4 vai trò.<br>- Quản lý kho thuốc (Nhập thuốc, cập nhật giá, tồn kho).<br>- Nhận sự kiện (Consume event) `prescription.created` để tự động trừ kho thuốc.<br>- Tính toán và thu viện phí (Phí khám + Tiền thuốc). | ASP.NET Core 8 Web API<br>SQL Server (`PharmacyDB`) |
| **Clinic Gateway** | Nhóm 5 | - Cổng kết nối chung (API Gateway) định tuyến mọi yêu cầu từ Frontend đến các dịch vụ Backend tương ứng.<br>- Tích hợp bộ lọc và xác thực Token JWT tập trung. | Ocelot Gateway (.NET 8/9) |
| **Clinic Frontend** | Nhóm 5 | - Giao diện người dùng duy nhất xử lý luồng công việc cho cả 4 vai trò: Bệnh nhân, Tiếp tân, Bác sĩ, Quản trị viên (Admin). | VueJS 3 + Vite |

---

## 3. Cấu trúc thư mục dự án trên branch

Thư mục dự án được tổ chức phân tách rõ ràng giữa các dịch vụ Backend và mã nguồn Frontend:

```text
AppointmentService/
    ├── AppointmentService/                  # Dịch vụ Đặt lịch & Phân luồng (Nhóm 5)
    │     ├── AppointmentService.API/         # Cổng API chính, Controllers, DTOs, cấu hình
    │     ├── AppointmentService.Domain/      # Các thực thể dữ liệu (Doctor, Schedule, Appointment)
    │     ├── AppointmentService.Infrastructure/ # Kết nối CSDL, Khởi tạo dữ liệu mẫu, Repositories
    │     └── AppointmentService.Tests/       # Bộ Unit Tests sử dụng xUnit và InMemory Database
    │
    ├── ClinicGateway/                       # Cổng kết nối API Gateway (Ocelot)
    │     ├── ocelot.json                    # Cấu hình định tuyến và phân quyền API
    │     └── Program.cs
    │
    ├── ClinicFrontend/                      # Giao diện VueJS 3 (Dùng chung cho cả hệ thống)
    │     ├── src/
    │     │    ├── views/
    │     │    │    ├── LandingPage.vue      # Giao diện trang chủ cho bệnh nhân tìm kiếm và đặt lịch
    │     │    │    ├── LoginPage.vue        # Giao diện đăng nhập tập trung
    │     │    │    └── DashboardPage.vue    # Giao diện phân quyền (Admin, Tiếp tân, Bác sĩ)
    │     │    ├── router/                   # Cấu hình định tuyến Frontend
    │     │    └── main.js
    │     └── package.json
    │
    ├── MedicalService/                      # Dịch vụ Quản lý bệnh án (Nhóm 4 - Tích hợp)
    │     ├── MedicalRecordService.Api/
    │     ├── MedicalRecordService.Data/
    │     └── MedicalRecordService.Models/
    │
    ├── PharmacyService/                     # Dịch vụ Kho thuốc & Thanh toán (Nhóm 6 - Tích hợp)
    │     ├── PharmacyBillingService/
    │     └── PharmacyBilling.sln
    │
    ├── docker-compose.yml                   # Cấu hình khởi chạy nhanh hệ thống cục bộ
    ├── clinic_workflow.md                   # Mô tả luồng hoạt động liên thông giữa 3 dịch vụ
    ├── needtofix.md                         # Ghi chú các điểm cần tối ưu hóa giao diện và logic
    └── README.md                            # Hướng dẫn dự án này
```

---

## 4. Hướng dẫn chạy thử dự án (Local Development)

### Cách 1: Chạy bằng Docker Compose (Khuyên dùng cho chạy thử và báo cáo)
Hệ thống Docker Compose đã được thiết lập sẵn để tự động cấu hình SQL Server, khởi tạo cơ sở dữ liệu mẫu và chạy đồng thời Appointment Service, API Gateway cùng Frontend.

1. Đảm bảo phần mềm Docker Desktop đã được khởi động trên máy tính của bạn.
2. Mở thư mục gốc `AppointmentService` bằng terminal.
3. Chạy lệnh sau để build và khởi động các container:
   ```bash
   docker-compose up --build
   ```
4. Khi quá trình khởi chạy hoàn tất:
   - **Giao diện Clinic Frontend**: Lắng nghe tại cổng **3000** (Truy cập: [http://localhost:3000](http://localhost:3000)).
   - **Cổng kết nối Gateway (Clinic Gateway)**: Lắng nghe tại cổng **8000** (Định tuyến API: `http://localhost:8000/api/appointments-service/{path}`).
   - **Dịch vụ Appointment API**: Chạy trực tiếp tại cổng **5000** (Tài liệu Swagger: [http://localhost:5000/index.html](http://localhost:5000/index.html)).

### Cách 2: Chạy trực tiếp bằng .NET CLI và NPM
Nếu bạn muốn chạy từng dịch vụ một cách độc lập để phát triển và debug:

1. **Khởi động các dịch vụ Backend**:
   - Chạy Appointment Service (Nhóm 5):
     ```bash
     dotnet run --project AppointmentService/AppointmentService.API/AppointmentService.API.csproj
     ```
   - Chạy Clinic Gateway:
     ```bash
     dotnet run --project ClinicGateway/ClinicGateway.csproj
     ```
   - Chạy Medical Record Service (Nhóm 4):
     ```bash
     dotnet run --project MedicalService/MedicalRecordService.Api/MedicalRecordService.Api.csproj
     ```
   - Chạy Pharmacy & Billing Service (Nhóm 6):
     ```bash
     dotnet run --project PharmacyService/PharmacyBillingService/PharmacyBillingService.csproj
     ```

2. **Khởi động mã nguồn Frontend**:
   - Di chuyển vào thư mục Frontend:
     ```bash
     cd ClinicFrontend
     ```
   - Cài đặt các thư viện phụ thuộc (chỉ thực hiện ở lần đầu tiên):
     ```bash
     npm install
     ```
   - Khởi chạy máy chủ phát triển Frontend:
     ```bash
     npm run dev
     ```
   - Truy cập giao diện người dùng tại cổng **3000** (mặc định: `http://localhost:3000`).

---

## 5. Cơ sở dữ liệu mẫu (Seed Data)

Khi dịch vụ `Appointment Service` khởi chạy lần đầu tiên, hệ thống sẽ tự động tạo cơ sở dữ liệu `AppointmentDB` và chèn dữ liệu mẫu bao gồm:
- **4 Bác sĩ**: Được cấu hình đầy đủ thông tin chuyên khoa, bằng cấp, phí khám.
- **Lịch làm việc**: Tự động tạo ca trực cho các bác sĩ trong vòng 7 ngày tiếp theo kể từ ngày hiện tại để đảm bảo luôn có ca khám trống khi thực hiện kiểm thử.
- **Lịch hẹn & Hàng chờ**: Một số lịch hẹn và hàng chờ mẫu để hiển thị trực quan trên giao diện Tiếp tân và Bác sĩ.

---

## 6. Hướng dẫn kiểm thử JWT Authentication và API

Hệ thống sử dụng cơ chế xác thực JWT Token dùng chung với khóa bí mật để kiểm tra quyền hạn của người dùng. Để thực hiện gọi các API yêu cầu quyền hạn trên Swagger mà không cần thông qua bước đăng nhập của Nhóm 6:

1. **Lấy Token kiểm thử**:
   - Trên tài liệu Swagger UI của Appointment Service (`http://localhost:5000/index.html`), tìm nhóm API `TestAuth`.
   - Thực hiện gọi API `GET /api/testauth/token` với tham số `role` mong muốn (ví dụ: `Admin`, `Receptionist`, `Doctor`, `Patient`).
   - Sao chép chuỗi mã thông báo JWT ở kết quả trả về.

2. **Áp dụng Token vào Swagger**:
   - Nhấn nút **Authorize** ở góc trên bên phải màn hình Swagger UI.
   - Nhập chuỗi theo định dạng: `Bearer {chuỗi_token_đã_copy}`.
   - Nhấn **Authorize** rồi nhấn **Close**. Bây giờ các yêu cầu gọi API của bạn sẽ được đính kèm token xác thực.

---

## 7. Hướng dẫn định dạng tệp CSV để Import (Tính năng của Admin)

Trang quản trị (Admin Dashboard) hỗ trợ tính năng tải lên hàng loạt danh sách Bác sĩ và Lịch trực thông qua tệp tin CSV để tiết kiệm thời gian nhập liệu.

### 7.1. Định dạng tệp CSV Bác sĩ (Doctors)
- **Cấu trúc các cột**: `Tên Bác sĩ, Chuyên khoa, Học vị/Bằng cấp, Phí khám`
- Dòng đầu tiên (Header) có thể chứa tên cột hoặc không. Nếu dòng đầu chứa chữ "Bác sĩ" hoặc "Name", hệ thống sẽ tự động bỏ qua.
- **Ví dụ nội dung file `bac_si.csv`**:
  ```text
  Nguyễn Văn A,Nội khoa,Thạc sĩ Bác sĩ,150000
  Trần Thị B,Nhi khoa,Bác sĩ Chuyên khoa 1,120000
  Lê Văn C,Da liễu,Bác sĩ Chuyên khoa 2,200000
  ```

### 7.2. Định dạng tệp CSV Lịch trực (Schedules)
- **Cấu trúc các cột**: `Tên Bác sĩ, Ngày trực (YYYY-MM-DD), Ca trực (Sang/Chieu/Toi), Số bệnh nhân tối đa`
- Hệ thống sẽ tự động tìm kiếm bác sĩ theo tên để liên kết lịch trực.
- **Ví dụ nội dung file `lich_truc.csv`**:
  ```text
  Nguyễn Văn A,2026-07-02,Sang,15
  Trần Thị B,2026-07-02,Chieu,10
  Lê Văn C,2026-07-02,Toi,8
  ```

---

## 8. Các API và chức năng nâng cao trên branch này

### 8.1. Chức năng giám sát hệ thống (Health Check)
- API Endpoint: `GET /health` (Chạy trực tiếp tại cổng 5000 hoặc qua Gateway cổng 8000).
- Trả về mã trạng thái HTTP 200 và chuỗi `Healthy` nếu cơ sở dữ liệu và dịch vụ hoạt động bình thường, giúp kiểm tra tính sẵn sàng của hệ thống.

### 8.2. Cải tiến giao diện Dashboard & Landing Page
- **Landing Page**: Thiết kế lại giao diện thân thiện với bệnh nhân, hỗ trợ chọn bác sĩ theo hàng ngang trực quan, hiển thị trực quan các khung giờ trống/đầy (khung giờ đã có người đặt hiển thị màu đỏ, khung giờ trống hiển thị màu xanh) để bệnh nhân dễ dàng lựa chọn.
- **Dashboard**: Tích hợp thanh điều hướng và tính năng đăng xuất ở góc trên bên phải trang web. Hỗ trợ đầy đủ chức năng phân trang (Pagination) cho danh sách lịch hẹn và bệnh nhân trong hàng chờ khám.

---

## 9. Hướng dẫn chạy Unit Tests

Dự án kiểm thử `AppointmentService.Tests` được tích hợp sẵn để kiểm tra các logic nghiệp vụ quan trọng như kiểm tra xung đột lịch khám, giới hạn số bệnh nhân, và tự động sinh số thứ tự xếp hàng.

Để chạy toàn bộ các bài kiểm thử, hãy mở terminal tại thư mục gốc và chạy lệnh:
```bash
dotnet test
```

---

## 10. Cấu hình Mạng LAN ảo (Radmin VPN) khi ghép nối lớp

Khi thực hiện báo cáo liên thông trực tiếp giữa 3 nhóm trên lớp học, các máy tính cần kết nối chung một mạng LAN ảo bằng Radmin VPN và cấu hình đúng địa chỉ IP đích:

- **Appointment Service (Nhóm 5 - Máy của bạn)**: Địa chỉ IP Radmin VPN là `26.88.31.108` (Cổng dịch vụ: `5000`).
- **Medical Record Service (Nhóm 4)**: Địa chỉ IP Radmin VPN là `26.15.45.202` (Gateway định tuyến yêu cầu bệnh án về địa chỉ này).
- **Pharmacy & Billing Service (Nhóm 6)**: Địa chỉ IP Radmin VPN là `26.71.15.204` (Mọi token JWT và hóa đơn được kiểm tra qua dịch vụ chạy tại địa chỉ này).
