using System;
using System.Collections.Generic;
using MedicalRecordService.Models.Enums;

namespace MedicalRecordService.Models.Entities
{
    public class PatientProfile
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        // Tiền sử bệnh, dị ứng (có thể null)
        public string? MedicalHistory { get; set; }
        public string? Allergies { get; set; }

        // Navigation property: một hồ sơ bệnh nhân có nhiều hồ sơ bệnh án
        public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();
    }
}
