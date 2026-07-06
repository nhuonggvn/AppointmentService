# HỆ THỐNG ĐẶT LỊCH & QUẢN LÝ PHÒNG KHÁM - CLINICFLOW
## BÀI TẬP LỚN PHÁT TRIỂN ỨNG DỤNG FULL STACK - ĐỀ TÀI 05

Dự án này là mã nguồn tích hợp toàn bộ hệ thống **ClinicFlow** (Đề tài 05 - Hệ thống đặt lịch & quản lý phòng khám) trong môn học Phát triển ứng dụng Full Stack. Hệ thống được phát triển theo kiến trúc **Microservices** phân tán kết hợp với cổng kết nối **API Gateway** và giao diện người dùng **Single Page Application (SPA)** đồng bộ.

Dự án bao gồm thành phần cốt lõi **Appointment Service** (Nhóm 5) cùng với mã nguồn tích hợp của các dịch vụ liên quan từ Nhóm 4 và Nhóm 6 nhằm phục vụ cho quá trình ghép nối, kiểm thử và vận hành liên thông.

Tài liệu này mô tả mã nguồn và các tính năng phát triển trên nhánh **`feature`** (được phát triển thêm và cải tiến từ nhánh `master`).

---

## 1. Các điểm phát triển thêm trên nhánh `feature` so với nhánh `master`

So với nhánh `master` chỉ có các thành phần API cơ bản, nhánh `feature` đã tích hợp đầy đủ giải pháp giao diện người dùng và tài liệu hóa quy trình nghiệp vụ:

### 1.1. Tích hợp và Khởi tạo dự án ClinicFrontend
- Khởi tạo dự án Frontend hoàn chỉnh sử dụng **VueJS 3 + Vite** và cài đặt các thư viện định tuyến (Vue Router).
- Xây dựng các trang giao diện nghiệp vụ đồng bộ bao gồm:
  - **Trang chủ Bệnh nhân (Landing Page)**: Tìm kiếm bác sĩ, lọc theo chuyên khoa và tiến hành đăng ký đặt lịch khám trực tuyến.
  - **Trang Đăng nhập (Login Page)**: Giao diện đăng nhập tập trung hỗ trợ phân quyền người dùng.
  - **Bảng điều khiển nghiệp vụ (Dashboard Page)**: Thiết kế giao diện quản trị phân quyền cho cả 4 vai trò (Admin, Tiếp tân, Bác sĩ, Bệnh nhân).

### 1.2. Bản vẽ sơ đồ quy trình liên thông hệ thống (clinic_workflow.md)
- Xây dựng tài liệu chi tiết mô tả luồng hoạt động nghiệp vụ y tế khép kín giữa 3 dịch vụ của 3 nhóm (Nhóm 4 - Bệnh án, Nhóm 5 - Đặt lịch & Hàng chờ, Nhóm 6 - Kho thuốc & Thanh toán).
- Tích hợp sơ đồ **Sequence Diagram (Mermaid)** trực quan thể hiện luồng tương tác thời gian thực từ lúc đặt lịch, đón tiếp tại quầy, khám bệnh kê đơn đến lúc tính tiền hóa đơn và phát thuốc ra về.

### 1.3. Tích hợp Mockup giao diện tĩnh (HTML/CSS & Ảnh màn hình)
- Lưu trữ toàn bộ mockup giao diện tại thư mục `stitch_microservices_clinic_management_system/`.
- Mỗi màn hình bao gồm file mã nguồn tĩnh (`code.html`) và hình ảnh trực quan (`screen.png`) cho 5 nghiệp vụ chính:
  - Trang chủ hệ thống (`trang_chu_clinicflow`).
  - Trang đăng nhập phân quyền (`ng_nh_p_clinicflow`).
  - Tiếp nhận, thu viện phí và quản lý hàng chờ (`tiep_nhan_thu_phi_quan_ly_hang_cho`).
  - Bác sĩ khám bệnh lâm sàng (`clinical_precision`).
  - Quản lý thông tin bác sĩ và lịch trực (`bac_si_va_lich_truc`).

---

## 2. Sự khác biệt của các nhánh

Nhánh `main` đóng vai trò là **Nhánh tích hợp hệ thống hoàn chỉnh (Integrated System)**. Dưới đây là bảng so sánh chi tiết các thành phần và tính năng giữa các nhánh:

| Thành phần / Tính năng | Nhánh `master` | Nhánh `feature` | Nhánh `main` |
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

## 3. Bản đồ trách nhiệm và kiến trúc hệ thống

Hệ thống hoạt động dựa trên sự phối hợp của 3 dịch vụ độc lập kết nối qua cổng API Gateway và giao tiếp bất đồng bộ qua Message Broker (RabbitMQ):

| Dịch vụ | Nhóm phát triển | Nhiệm vụ chính | Công nghệ & CSDL |
| :--- | :--- | :--- | :--- |
| **Appointment Service** | Nhóm 5 (Cốt lõi) | - Quản lý hồ sơ bác sĩ, ca trực, chuyên khoa, phí khám.<br>- Đặt lịch khám phía Bệnh nhân (lọc chuyên khoa, bác sĩ, khung giờ trống).<br>- Phát hiện trùng giờ khám (Conflict slot).<br>- Tiếp tân duyệt lịch hẹn và cấp Số thứ tự (STT) vào Hàng chờ (Queue). | ASP.NET Core 9 Web API<br>SQL Server (`AppointmentDB`) |
| **Medical Record Service** | Nhóm 4 | - Quản lý thông tin hành chính, tiền sử bệnh án, dị ứng của Bệnh nhân.<br>- Bác sĩ ghi nhận triệu chứng, chẩn đoán bệnh án.<br>- Kê đơn thuốc từ danh mục thuốc liên thông.<br>- Phát đi sự kiện (Publish event) `prescription.created` sau khi hoàn thành. | ASP.NET Core 8 Web API<br>SQL Server (`MedicalDB`) |
| **Pharmacy & Billing Service** | Nhóm 6 | - Xác thực tập trung (JWT login), quản lý tài khoản cho cả 4 vai trò.<br>- Quản lý kho thuốc (Nhập thuốc, cập nhật giá, tồn kho).<br>- Nhận sự kiện (Consume event) `prescription.created` để tự động trừ kho thuốc.<br>- Tính toán và thu viện phí (Phí khám + Tiền thuốc). | ASP.NET Core 8 Web API<br>SQL Server (`PharmacyDB`) |
| **Clinic Gateway** | Nhóm 5 | - Cổng kết nối chung (API Gateway) định tuyến mọi yêu cầu từ Frontend đến các dịch vụ Backend tương ứng.<br>- Tích hợp bộ lọc và xác thực Token JWT tập trung. | Ocelot Gateway (.NET 8/9) |
| **Clinic Frontend** | Nhóm 5 | - Giao diện người dùng duy nhất xử lý luồng công việc cho cả 4 vai trò: Bệnh nhân, Tiếp tân, Bác sĩ, Quản trị viên (Admin). | VueJS 3 + Vite |

---

## 4. Cấu trúc thư mục dự án

Thư mục dự án được tổ chức phân tách rõ ràng giữa các dịch vụ Backend, mã nguồn Frontend và các mockup tĩnh hỗ trợ:

```text
AppointmentService/
    AppointmentService/                      # Dịch vụ Đặt lịch & Phân luồng (Nhóm 5 Backend)
        AppointmentService.API/         # API chính, Controllers, DTOs, cấu hình
        AppointmentService.Domain/      # Thực thể dữ liệu (Doctor, Schedule, Appointment)
        AppointmentService.Infrastructure/ # Cấu hình CSDL, Khởi tạo dữ liệu, Repositories
        AppointmentService.Tests/       # Bộ kiểm thử tự động xUnit (InMemory Database)

    ClinicGateway/                       # Cổng kết nối API Gateway (Ocelot)
        ocelot.json                    # Cấu hình định tuyến và xác thực API
        Program.cs

    ClinicFrontend/                      # Giao diện VueJS 3 cho hệ thống ClinicFlow
        src/
            views/
                LandingPage.vue      # Giao diện trang chủ đặt lịch của Bệnh nhân
                LoginPage.vue        # Giao diện đăng nhập tập trung
                DashboardPage.vue    # Giao diện nghiệp vụ Tiếp tân, Bác sĩ, Admin
            router/                   # Cấu hình định tuyến Frontend
            main.js
        package.json

    MedicalService/                      # Dịch vụ Quản lý bệnh án (Nhóm 4 - Tích hợp)
        MedicalRecordService.Api/
        MedicalRecordService.Data/
        MedicalRecordService.Models/

    PharmacyService/                     # Dịch vụ Kho thuốc & Thanh toán (Nhóm 6 - Tích hợp)
        PharmacyBillingService/
        PharmacyBilling.sln

    stitch_microservices_clinic_management_system/ # Bộ Mockup giao diện tĩnh
        trang_chu_clinicflow/          # File code.html và ảnh chụp screen.png
        ng_nh_p_clinicflow/            # File code.html và ảnh chụp screen.png
        tiep_nhan_thu_phi_quan_ly_hang_cho/ # File code.html và ảnh chụp screen.png
        clinical_precision/            # File code.html, DESIGN.md và ảnh chụp screen.png
        bac_si_va_lich_truc/           # File code.html và ảnh chụp screen.png

    docker-compose.yml                   # Cấu hình khởi chạy nhanh hệ thống bằng Docker Compose
    clinic_workflow.md                   # Luồng hoạt động liên thông hệ thống
    needtofix.md                         # Ghi chú các điểm cần tối ưu hóa giao diện và logic
    README.md                            # Hướng dẫn dự án
```

---

## 5. Hướng dẫn chạy thử dự án (Local Development)

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
   - Truy cập giao diện người dùng tại cổng **3000** (mặc định: `http://localhost:3000`). Bạn có thể sử dụng nút **"Cấu hình kết nối"** ở góc trên bên phải màn hình để thiết lập URL kết nối API.

---

## 6. Cơ sở dữ liệu mẫu (Seed Data)

Khi dịch vụ `Appointment Service` khởi chạy lần đầu tiên, hệ thống sẽ tự động tạo cơ sở dữ liệu `AppointmentDB` và chèn dữ liệu mẫu bao gồm:
- **4 Bác sĩ**: Được cấu hình đầy đủ thông tin chuyên khoa, bằng cấp, phí khám.
- **Lịch làm việc**: Tự động tạo ca trực cho các bác sĩ trong vòng 7 ngày tiếp theo kể từ ngày hiện tại để đảm bảo luôn có ca khám trống khi thực hiện kiểm thử.
- **Lịch hẹn & Hàng chờ**: Một số lịch hẹn và hàng chờ mẫu để hiển thị trực quan trên giao diện Tiếp tân và Bác sĩ.

---

## 7. Hướng dẫn kiểm thử JWT Authentication và API

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

## 8. Hướng dẫn định dạng tệp CSV để Import (Tính năng của Admin)

Trang quản trị (Admin Dashboard) hỗ trợ tính năng tải lên hàng loạt danh sách Bác sĩ và Lịch trực thông qua tệp tin CSV để tiết kiệm thời gian nhập liệu.

### 8.1. Định dạng tệp CSV Bác sĩ (Doctors)
- **Cấu trúc các cột**: `Tên Bác sĩ, Chuyên khoa, Học vị/Bằng cấp, Phí khám`
- Dòng đầu tiên (Header) có thể chứa tên cột hoặc không. Nếu dòng đầu chứa chữ "Bác sĩ" hoặc "Name", hệ thống sẽ tự động bỏ qua.
- **Ví dụ nội dung file `bac_si.csv`**:
  ```text
  Nguyễn Văn A,Nội khoa,Thạc sĩ Bác sĩ,150000
  Trần Thị B,Nhi khoa,Bác sĩ Chuyên khoa 1,120000
  Lê Văn C,Da liễu,Bác sĩ Chuyên khoa 2,200000
  ```

### 8.2. Định dạng tệp CSV Lịch trực (Schedules)
- **Cấu trúc các cột**: `Tên Bác sĩ, Ngày trực (YYYY-MM-DD), Ca trực (Sang/Chieu/Toi), Số bệnh nhân tối đa`
- Hệ thống sẽ tự động tìm kiếm bác sĩ theo tên để liên kết lịch trực.
- **Ví dụ nội dung file `lich_truc.csv`**:
  ```text
  Nguyễn Văn A,2026-07-02,Sang,15
  Trần Thị B,2026-07-02,Chieu,10
  Lê Văn C,2026-07-02,Toi,8
  ```

---

## 9. Các API và chức năng nâng cao trên branch này

### 9.1. Chức năng giám sát hệ thống (Health Check)
- API Endpoint: `GET /health` (Chạy trực tiếp tại cổng 5000 hoặc qua Gateway cổng 8000).
- Trả về mã trạng thái HTTP 200 và chuỗi `Healthy` nếu cơ sở dữ liệu và dịch vụ hoạt động bình thường, giúp kiểm tra tính sẵn sàng của hệ thống.

### 9.2. Cải tiến giao diện Dashboard & Landing Page
- **Landing Page**: Thiết kế lại giao diện thân thiện với bệnh nhân, hỗ trợ chọn bác sĩ theo hàng ngang trực quan, hiển thị trực quan các khung giờ trống/đầy (khung giờ đã có người đặt hiển thị màu đỏ, khung giờ trống hiển thị màu xanh) để bệnh nhân dễ dàng lựa chọn.
- **Dashboard**: Tích hợp thanh điều hướng và tính năng đăng xuất ở góc trên bên phải trang web. Hỗ trợ đầy đủ chức năng phân trang (Pagination) cho danh sách lịch hẹn và bệnh nhân trong hàng chờ khám.

---

## 10. Hướng dẫn chạy Unit Tests

Dự án kiểm thử `AppointmentService.Tests` được tích hợp sẵn để kiểm tra các logic nghiệp vụ quan trọng như kiểm tra xung đột lịch khám, giới hạn số bệnh nhân, và tự động sinh số thứ tự xếp hàng.

Để chạy toàn bộ các bài kiểm thử, hãy mở terminal tại thư mục gốc và chạy lệnh:
```bash
dotnet test
```

---

## 11. Cấu hình Mạng LAN ảo (Radmin VPN) khi ghép nối lớp

Khi thực hiện báo cáo liên thông trực tiếp giữa 3 nhóm trên lớp học, các máy tính cần kết nối chung một mạng LAN ảo bằng Radmin VPN và cấu hình đúng địa chỉ IP đích:

- **Appointment Service (Nhóm 5 - Máy của bạn)**: Địa chỉ IP Radmin VPN là `26.88.31.108` (Cổng dịch vụ: `5000`).
- **Medical Record Service (Nhóm 4)**: Địa chỉ IP Radmin VPN là `26.79.10.201` (Gateway định tuyến yêu cầu bệnh án về địa chỉ này).
- **Pharmacy & Billing Service (Nhóm 6)**: Địa chỉ IP Radmin VPN là `26.71.15.204` (Mọi token JWT và hóa đơn được kiểm tra qua dịch vụ chạy tại địa chỉ này).

### Hướng dẫn kiểm tra nhanh kết nối giữa các nhóm:
1. Mở Command Prompt hoặc PowerShell trên máy và sử dụng lệnh ping để kiểm tra kết nối mạng ảo tới các nhóm khác:
   - Kiểm tra kết nối tới Nhóm 4: `ping 26.79.10.201`
   - Kiểm tra kết nối tới Nhóm 6: `ping 26.71.15.204`
2. Đảm bảo rằng tệp cấu hình của Gateway và dự án Frontend dùng chung đã cấu hình đúng các địa chỉ IP này cho các API tương ứng.
