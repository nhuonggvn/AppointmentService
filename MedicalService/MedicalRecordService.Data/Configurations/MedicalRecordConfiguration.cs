using MedicalRecordService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalRecordService.Data.Configurations
{
    public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
    {
        public void Configure(EntityTypeBuilder<MedicalRecord> builder)
        {
            builder.ToTable("MedicalRecords");

            builder.HasKey(m => m.Id);

            // Khóa ngoại nội bộ trỏ đến PatientProfile
            builder.Property(m => m.PatientProfileId)
                .IsRequired();

            // Các cột tham chiếu ngoài từ các service khác (không tạo FK cứng hay navigation property)
            builder.Property(m => m.DoctorId)
                .IsRequired();

            builder.Property(m => m.AppointmentId)
                .IsRequired();

            builder.Property(m => m.ExaminationDate)
                .IsRequired();

            builder.Property(m => m.Symptoms)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.Diagnosis)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(m => m.DoctorNotes)
                .HasMaxLength(1000);

            // Các index để truy vấn nhanh
            builder.HasIndex(m => m.PatientProfileId)
                .HasDatabaseName("IX_MedicalRecords_PatientProfileId");

            builder.HasIndex(m => m.DoctorId)
                .HasDatabaseName("IX_MedicalRecords_DoctorId");

            builder.HasIndex(m => m.AppointmentId)
                .HasDatabaseName("IX_MedicalRecords_AppointmentId");

            builder.HasIndex(m => m.ExaminationDate)
                .HasDatabaseName("IX_MedicalRecords_ExaminationDate");

            // Cấu hình quan hệ nội bộ 1-1 với Prescription
            builder.HasOne(m => m.Prescription)
                .WithOne(p => p.MedicalRecord)
                .HasForeignKey<Prescription>(p => p.MedicalRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}