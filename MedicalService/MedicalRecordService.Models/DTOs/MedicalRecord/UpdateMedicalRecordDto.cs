using System;

namespace MedicalRecordService.Models.DTOs.MedicalRecord
{
    public class UpdateMedicalRecordDto
    {
        public Guid DoctorId { get; set; }
        public Guid AppointmentId { get; set; }
        public DateTime ExaminationDate { get; set; }
        public string Symptoms { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string? DoctorNotes { get; set; }
    }
}