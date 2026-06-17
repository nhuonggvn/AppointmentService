using System;
using AppointmentService.Domain.Enums;

namespace AppointmentService.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public string AppointmentCode { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;
        public string PatientEmail { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public TimeSpan TimeSlot { get; set; }
        public Guid DoctorId { get; set; }
        public Guid ScheduleId { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.ChoXacNhan;
        public int? QueueNumber { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Doctor? Doctor { get; set; }
        public DoctorSchedule? Schedule { get; set; }
    }
}
