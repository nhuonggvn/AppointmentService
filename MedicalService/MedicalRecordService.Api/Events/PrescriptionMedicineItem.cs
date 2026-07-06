using System;

namespace MedicalRecordService.Api.Events
{
    public class PrescriptionMedicineItem
    {
        public PrescriptionMedicineItem(Guid medicineId, int quantity)
        {
            MedicineId = medicineId;
            Quantity = quantity;
        }

        public Guid MedicineId { get; set; }
        public int Quantity { get; set; }
    }
}
