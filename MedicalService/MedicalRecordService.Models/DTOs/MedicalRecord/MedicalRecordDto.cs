using System;
using MedicalRecordService.Models.DTOs.Prescription;

namespace MedicalRecordService.Models.DTOs.MedicalRecord
{
    public class MedicalRecordDto
    {
        public Guid Id { get; set; }
        public Guid PatientProfileId { get; set; }
        public string PatientFullName { get; set; } = string.Empty;
        public Guid DoctorId { get; set; }
        public Guid AppointmentId { get; set; }
        public DateTime ExaminationDate { get; set; }
        public string Symptoms { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string? DoctorNotes { get; set; }
        public PrescriptionDto? Prescription { get; set; }
    }
}