using System;

namespace AppointmentService.API.DTOs
{
    public class AppointmentDto
    {
        public Guid Id { get; set; }
        public string AppointmentCode { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public string PatientEmail { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public Guid DoctorId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public Guid ScheduleId { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? QueueNumber { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class BookAppointmentDto
    {
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public string PatientEmail { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty; // Format "hh:mm:ss"
        public Guid DoctorId { get; set; }
        public Guid ScheduleId { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
