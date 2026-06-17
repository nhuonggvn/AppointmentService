using System;

namespace AppointmentService.API.DTOs
{
    public class ScheduleDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Shift { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public int MaxPatients { get; set; }
        public int CurrentBookings { get; set; }
        public bool IsAvailable => CurrentBookings < MaxPatients;
    }

    public class CreateScheduleDto
    {
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public int Shift { get; set; } // 1: Sang, 2: Chieu, 3: Toi
        public string StartTime { get; set; } = "08:00:00"; // TimeSpan string format "hh:mm:ss"
        public string EndTime { get; set; } = "11:30:00";
        public int MaxPatients { get; set; }
    }
}
