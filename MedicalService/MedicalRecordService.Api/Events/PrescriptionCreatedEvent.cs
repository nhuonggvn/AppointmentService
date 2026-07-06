using System;
using System.Collections.Generic;

namespace MedicalRecordService.Api.Events
{
    public class PrescriptionCreatedEvent
    {
        public PrescriptionCreatedEvent(
            Guid prescriptionId,
            Guid medicalRecordId,
            List<PrescriptionMedicineItem> medicines,
            DateTime createdAt)
        {
            PrescriptionId = prescriptionId;
            MedicalRecordId = medicalRecordId;
            Medicines = medicines ?? new List<PrescriptionMedicineItem>();
            CreatedAt = createdAt;
        }

        public Guid PrescriptionId { get; set; }
        public Guid MedicalRecordId { get; set; }
        public List<PrescriptionMedicineItem> Medicines { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}