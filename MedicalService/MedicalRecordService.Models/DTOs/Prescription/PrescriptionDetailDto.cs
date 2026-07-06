using System;

namespace MedicalRecordService.Models.DTOs.Prescription
{
    public class PrescriptionDetailDto
    {
        public Guid Id { get; set; }
        public Guid MedicineId { get; set; }
        public int Quantity { get; set; }
        public string DosageInstruction { get; set; } = string.Empty;
    }
}