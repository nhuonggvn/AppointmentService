using System;
using AppointmentService.Domain.Enums;

namespace AppointmentService.Domain.Entities
{
    public class DoctorSchedule
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public ShiftType Shift { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int MaxPatients { get; set; }
        public int CurrentBookings { get; set; }

        // Navigation property
        public Doctor? Doctor { get; set; }
    }
}
