using MedicalRecordService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalRecordService.Data.Configurations
{
    public class PatientProfileConfiguration : IEntityTypeConfiguration<PatientProfile>
    {
        public void Configure(EntityTypeBuilder<PatientProfile> builder)
        {
            builder.ToTable("PatientProfiles");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.DateOfBirth)
                .IsRequired();

            builder.Property(p => p.Gender)
                .IsRequired();

            builder.Property(p => p.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(p => p.Address)
                .HasMaxLength(200);

            builder.Property(p => p.MedicalHistory)
                .HasMaxLength(500);

            builder.Property(p => p.Allergies)
                .HasMaxLength(500);

            // Index cho PhoneNumber
            builder.HasIndex(p => p.PhoneNumber)
                .HasDatabaseName("IX_PatientProfiles_PhoneNumber");

            // Quan hệ nội bộ: PatientProfile (1) -> MedicalRecord (N)
            builder.HasMany(p => p.MedicalRecords)
                .WithOne(m => m.PatientProfile)
                .HasForeignKey(m => m.PatientProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
