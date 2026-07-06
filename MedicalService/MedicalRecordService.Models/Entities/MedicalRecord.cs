using System;

namespace MedicalRecordService.Models.Entities
{
    public class MedicalRecord
    {
        public Guid Id { get; set; }

        // Khóa ngoại nội bộ trỏ đến PatientProfile
        public Guid PatientProfileId { get; set; }
        public virtual PatientProfile PatientProfile { get; set; } = null!;

        // Tham chiếu ngoài từ Appointment Service (không có navigation property hay FK cứng)
        public Guid DoctorId { get; set; }
        public Guid AppointmentId { get; set; }

        public DateTime ExaminationDate { get; set; }

        public string Symptoms { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string? DoctorNotes { get; set; }

        // Một hồ sơ có thể có một đơn thuốc (hoặc không)
        public virtual Prescription? Prescription { get; set; }
    }
}