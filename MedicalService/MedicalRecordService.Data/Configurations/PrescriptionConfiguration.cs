using MedicalRecordService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalRecordService.Data.Configurations
{
    public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
    {
        public void Configure(EntityTypeBuilder<Prescription> builder)
        {
            builder.ToTable("Prescriptions");

            builder.HasKey(p => p.Id);

            // MedicalRecordId là khóa ngoại nội bộ
            builder.Property(p => p.MedicalRecordId)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.Notes)
                .HasMaxLength(500);

            // Index cho MedicalRecordId
            builder.HasIndex(p => p.MedicalRecordId)
                .HasDatabaseName("IX_Prescriptions_MedicalRecordId");

            // Quan hệ với PrescriptionDetail (một Prescription có nhiều Detail)
            builder.HasMany(p => p.Details)
                .WithOne(d => d.Prescription)
                .HasForeignKey(d => d.PrescriptionId)
                .OnDelete(DeleteBehavior.Cascade); // Khi xóa Prescription, xóa luôn Details
        }
    }
}