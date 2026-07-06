using System;
using System.Collections.Generic;

namespace MedicalRecordService.Models.Entities
{
    public class Prescription
    {
        public Guid Id { get; set; }

        // Khóa ngoại nội bộ trỏ đến MedicalRecord
        public Guid MedicalRecordId { get; set; }
        public virtual MedicalRecord MedicalRecord { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string? Notes { get; set; }

        // Danh sách chi tiết đơn thuốc
        public virtual ICollection<PrescriptionDetail> Details { get; set; } = new List<PrescriptionDetail>();
    }
}