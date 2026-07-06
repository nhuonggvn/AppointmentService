using MedicalRecordService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalRecordService.Data.DbContext
{
    public class MedicalDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public MedicalDbContext(DbContextOptions<MedicalDbContext> options)
            : base(options)
        {
        }

                // DbSet cho từng Entity (tương ứng với bảng trong DB)
        public DbSet<PatientProfile> PatientProfiles { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionDetail> PrescriptionDetails { get; set; }

        // Cấu hình Fluent API cho các Entity
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Áp dụng tất cả cấu hình từ các file Configuration (sẽ tạo ở bước sau)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MedicalDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}