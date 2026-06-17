# BÀI TẬP LỚN FULL STACK - NHÓM 5 - ĐỀ TÀI 05
## Dịch vụ Đặt lịch khám (Appointment Service)

Dự án này là thành phần **Appointment Service** (nhóm 5) thuộc đề tài 5 **"Hệ thống đặt lịch & quản lý phòng khám"** trong môn học Phát triển ứng dụng Full Stack. Dịch vụ được phát triển bằng **ASP.NET Core 9 Web API** và cơ sở dữ liệu **SQL Server**.

---

## 1. Cấu trúc thư mục dự án

Dự án được tổ chức theo kiến trúc phân tầng sạch sẽ (N-Tier Architecture) giúp phân tách rõ ràng trách nhiệm giữa các thành phần:

```text
BTL_FullStrack/
    AppointmentService/
        AppointmentService.API/
            Controllers/
                AppointmentsController.cs
                DoctorsController.cs
                QueueController.cs
                SchedulesController.cs
                TestAuthController.cs
            DTOs/
                DoctorDto.cs
                ScheduleDto.cs
                AppointmentDto.cs
                QueueDto.cs
            Program.cs
            appsettings.json
            appsettings.Development.json
            Dockerfile
        AppointmentService.Domain/
            Entities/
                Doctor.cs
                DoctorSchedule.cs
                Appointment.cs
                ReceptionQueue.cs
            Enums/
                AppointmentStatus.cs
                QueueStatus.cs
                ShiftType.cs
        AppointmentService.Infrastructure/
            Data/
                AppointmentDbContext.cs
                DbInitializer.cs
            Repositories/
                Interfaces/
                    IRepository.cs
                Implementations/
                    Repository.cs
        AppointmentService.Tests/
            AppointmentServiceTests.cs
    docker-compose.yml
    README.md
```

---

## 2. Hướng dẫn chạy thử dự án (Local Development)

### Cách 1: Chạy bằng Docker Compose (Khuyên dùng khi báo cáo / liên kết nhóm)
Docker Compose sẽ tự động khởi chạy SQL Server và build dự án API, liên kết chúng lại với nhau mà bạn không cần phải cài đặt SQL Server cục bộ trên máy.

1. Đảm bảo Docker Desktop đã được bật.
2. Mở thư mục gốc `BTL_FullStrack` bằng terminal (PowerShell hoặc Cmd).
3. Chạy lệnh sau để khởi động hệ thống:
   ```bash
   docker-compose up --build
   ```
4. Khi chạy thành công, dịch vụ Web API sẽ lắng nghe tại cổng **5000**.
5. Truy cập Swagger UI tại địa chỉ: [http://localhost:5000/index.html](http://localhost:5000/index.html)

### Cách 2: Chạy trực tiếp từ Visual Studio hoặc .NET CLI
1. **Cấu hình Database**: Mở file `appsettings.json` trong `AppointmentService.API` và chỉnh sửa chuỗi kết nối `"DefaultConnection"` sao cho phù hợp với máy chủ SQL Server cục bộ của bạn.
2. **Khởi chạy API**:
   - Bằng Visual Studio: Mở Solution `AppointmentService.slnx` hoặc các dự án con, thiết lập `AppointmentService.API` làm Startup Project và nhấn F5.
   - Bằng .NET CLI: Chạy lệnh sau tại thư mục gốc:
     ```bash
     dotnet run --project AppointmentService/AppointmentService.API/AppointmentService.API.csproj
     ```
3. Truy cập Swagger UI tại cổng tương ứng được hiển thị trên console (mặc định là cổng HTTPS/HTTP được cấu hình trong launchSettings, hoặc root URL `/` để mở Swagger).

---

## 3. Cơ sở dữ liệu mẫu (Seed Data)
Dự án được cấu hình tự động tạo Cơ sở dữ liệu và chèn dữ liệu mẫu (Seed Data) ngay khi khởi chạy lần đầu tiên. Dữ liệu mẫu bao gồm:
- **4 Bác sĩ**: Cấu hình đầy đủ thông tin chuyên khoa, bằng cấp, phí khám.
- **Lịch làm việc**: Tự động sinh lịch làm việc cho 4 bác sĩ trong vòng 7 ngày tiếp theo kể từ ngày hiện tại (đảm bảo luôn có ca khám trống khi test).
- **Lịch hẹn & Hàng chờ**: Một số lịch hẹn mẫu và hàng chờ tiếp nhận để kiểm tra danh sách hiển thị.

---

## 4. Hướng dẫn kiểm thử JWT Authentication và API trên Swagger

Vì kiến trúc hệ thống yêu cầu xác thực JWT qua API Gateway với **Shared Secret Key**, dự án đã được tích hợp bộ lọc Swagger hỗ trợ gửi Token. Để kiểm thử độc lập mà không cần khởi chạy Auth Service của nhóm khác, bạn thực hiện như sau:

1. **Lấy Token kiểm thử**:
   - Trên Swagger UI, tìm nhóm API `TestAuth`.
   - Gọi API `GET /api/testauth/token` với tham số `role` mong muốn (ví dụ: `Admin`, `Receptionist`, `Doctor`, `Patient`).
   - Sao chép chuỗi mã thông báo JWT trong trường `token` ở kết quả trả về.

2. **Áp dụng Token vào Swagger**:
   - Cuộn lên đầu giao diện Swagger, nhấn nút **Authorize** ở góc bên phải.
   - Trong ô giá trị, nhập: `Bearer {chuỗi_token_đã_copy}` (Ví dụ: `Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...`).
   - Nhấn **Authorize** rồi nhấn **Close**.
   - Bây giờ bạn có thể gọi các API yêu cầu quyền hạn cao như cấu hình lịch làm việc (Admin), hoặc xác nhận lịch hẹn (Receptionist) bình thường.

---

## 5. Danh sách các API chính

### Quản lý Bác sĩ (`Doctors`)
- `GET /api/doctors`: Lấy danh sách bác sĩ hoạt động (Cho phép lọc theo chuyên khoa).
- `GET /api/doctors/{id}`: Chi tiết bác sĩ.
- `POST /api/doctors` [Quyền: Admin]: Thêm bác sĩ mới.
- `PUT /api/doctors/{id}` [Quyền: Admin]: Sửa thông tin bác sĩ.
- `DELETE /api/doctors/{id}` [Quyền: Admin]: Đổi trạng thái hoạt động (Soft Delete).

### Quản lý Lịch làm việc (`Schedules`)
- `GET /api/schedules` [Quyền: Admin, Receptionist]: Xem tất cả lịch làm việc của bác sĩ theo ngày.
- `GET /api/schedules/available`: Xem danh sách ca làm việc còn trống của các bác sĩ kể từ ngày hôm nay (dành cho bệnh nhân đặt lịch).
- `POST /api/schedules` [Quyền: Admin]: Tạo lịch làm việc mới cho bác sĩ.

### Đặt lịch hẹn (`Appointments`)
- `POST /api/appointments/book`: Bệnh nhân đăng ký đặt lịch khám. Hệ thống tự động kiểm tra trùng ca khám (Conflict Slot), kiểm tra giới hạn số bệnh nhân tối đa trong ca và kiểm tra đăng ký lặp để trả về lỗi hợp lý.
- `GET /api/appointments/pending` [Quyền: Admin, Receptionist]: Lấy danh sách lịch hẹn đang chờ tiếp tân xác nhận.
- `GET /api/appointments/search`: Tìm kiếm lịch hẹn của bệnh nhân theo Số điện thoại hoặc Mã lịch hẹn.
- `PUT /api/appointments/{id}/cancel`: Hủy lịch hẹn (Nếu lịch hẹn đã được xác nhận trước đó, ca khám của bác sĩ sẽ tự động được hoàn lại 1 chỗ trống).

### Hàng chờ tiếp nhận (`Queue`)
- `POST /api/queue/confirm-appointment/{appointmentId}` [Quyền: Admin, Receptionist]: Tiếp tân xác nhận lịch hẹn của bệnh nhân. Trạng thái lịch hẹn chuyển sang `DaXacNhan`, hệ thống tự động sinh số thứ tự xếp hàng khám (Queue Number) tăng dần theo bác sĩ trong ngày, đồng thời tạo một bản ghi hàng chờ ở trạng thái `ChoKham`.
- `GET /api/queue/active`: Lấy danh sách hàng chờ hiện tại trong ngày của các bác sĩ (chỉ hiển thị những ca đang ở trạng thái `ChoKham` hoặc `DangKham` để đưa lên màn hình tivi hàng chờ).
- `PUT /api/queue/{id}/status` [Quyền: Admin, Receptionist, Doctor]: Cập nhật trạng thái hàng chờ khám (1: ChoKham, 2: DangKham, 3: DaHoanThanh, 4: BoQua). Khi cập nhật trạng thái này, trạng thái của lịch hẹn gốc tương ứng cũng sẽ tự động được cập nhật đồng bộ sang `DangKham` hoặc `DaKham`.

---

## 6. Hướng dẫn chạy Unit Tests

Dự án kiểm thử `AppointmentService.Tests` chứa các kịch bản kiểm thử tự động sử dụng **xUnit** và cơ sở dữ liệu ảo **InMemory Database** để kiểm tra tính chính xác của các logic nghiệp vụ quan trọng.

Để chạy các bài kiểm tra, mở terminal tại thư mục gốc và chạy lệnh:
```bash
dotnet test
```
Kết quả hiển thị `Passed! - Failed: 0` biểu thị toàn bộ logic đặt lịch khám và xếp số thứ tự hàng chờ đều hoạt động chính xác tuyệt đối.
