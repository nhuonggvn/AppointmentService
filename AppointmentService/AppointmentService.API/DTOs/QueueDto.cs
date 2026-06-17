using System;

namespace AppointmentService.API.DTOs
{
    public class QueueDto
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int QueueNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CheckInTime { get; set; }
    }

    public class UpdateQueueStatusDto
    {
        public int Status { get; set; } // 1: ChoKham, 2: DangKham, 3: BoQua, 4: DaHoanThanh
    }
}
