using System;
using System.Collections.Generic;

namespace MedicalRecordService.Models.DTOs.Prescription
{
    public class CreatePrescriptionDto
    {
        public Guid MedicalRecordId { get; set; }
        public string? Notes { get; set; }
        public List<CreatePrescriptionDetailDto> Details { get; set; } = new();
    }
}