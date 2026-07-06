using MedicalRecordService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalRecordService.Data.Configurations
{
    public class PrescriptionDetailConfiguration : IEntityTypeConfiguration<PrescriptionDetail>
    {
        public void Configure(EntityTypeBuilder<PrescriptionDetail> builder)
        {
            builder.ToTable("PrescriptionDetails");

            builder.HasKey(pd => pd.Id);

            // Khóa ngoại nội bộ trỏ đến Prescription
            builder.Property(pd => pd.PrescriptionId)
                .IsRequired();

            // Tham chiếu ngoài từ Pharmacy Service (chỉ lưu ID dữ liệu thông thường)
            builder.Property(pd => pd.MedicineId)
                .IsRequired();

            builder.Property(pd => pd.Quantity)
                .IsRequired();

            builder.Property(pd => pd.DosageInstruction)
                .IsRequired()
                .HasMaxLength(500);

            // Index
            builder.HasIndex(pd => pd.MedicineId)
                .HasDatabaseName("IX_PrescriptionDetails_MedicineId");

            builder.HasIndex(pd => pd.PrescriptionId)
                .HasDatabaseName("IX_PrescriptionDetails_PrescriptionId");
        }
    }
}