using System;
using AppointmentService.Domain.Enums;

namespace AppointmentService.Domain.Entities
{
    public class ReceptionQueue
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public int QueueNumber { get; set; }
        public QueueStatus Status { get; set; } = QueueStatus.ChoKham;
        public DateTime CheckInTime { get; set; } = DateTime.UtcNow;

        // Navigation property
        public Appointment? Appointment { get; set; }
    }
}
