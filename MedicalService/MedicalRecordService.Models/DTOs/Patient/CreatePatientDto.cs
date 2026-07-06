using System;
using MedicalRecordService.Models.Enums;

namespace MedicalRecordService.Models.DTOs.Patient
{
    public class CreatePatientDto
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? MedicalHistory { get; set; }
        public string? Allergies { get; set; }
    }
}