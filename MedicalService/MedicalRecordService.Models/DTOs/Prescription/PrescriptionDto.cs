using System;
using System.Collections.Generic;

namespace MedicalRecordService.Models.DTOs.Prescription
{
    public class PrescriptionDto
    {
        public Guid Id { get; set; }
        public Guid MedicalRecordId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Notes { get; set; }
        public List<PrescriptionDetailDto> Details { get; set; } = new();
    }
}