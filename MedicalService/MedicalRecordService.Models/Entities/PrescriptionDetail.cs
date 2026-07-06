using System;

namespace MedicalRecordService.Models.Entities
{
    public class PrescriptionDetail
    {
        public Guid Id { get; set; }

        // Khóa ngoại nội bộ trỏ đến Prescription
        public Guid PrescriptionId { get; set; }
        public virtual Prescription Prescription { get; set; } = null!;

        // Tham chiếu ngoài từ Pharmacy Service (Medicine)
        public Guid MedicineId { get; set; }

        public int Quantity { get; set; }

        public string DosageInstruction { get; set; } = string.Empty;
    }
}