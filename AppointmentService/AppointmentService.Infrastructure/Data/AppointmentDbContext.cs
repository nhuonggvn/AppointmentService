using Microsoft.EntityFrameworkCore;
using AppointmentService.Domain.Entities;

namespace AppointmentService.Infrastructure.Data
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; } = null!;
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; } = null!;
        public DbSet<Appointment> Appointments { get; set; } = null!;
        public DbSet<ReceptionQueue> ReceptionQueues { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Doctor entity
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctors");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.FullName).IsRequired().HasMaxLength(150);
                entity.Property(d => d.Specialty).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Qualifications).HasMaxLength(250);
                entity.Property(d => d.ConsultationFee).HasPrecision(18, 2);
                entity.Property(d => d.IsActive).HasDefaultValue(true);
            });

            // Configure DoctorSchedule entity
            modelBuilder.Entity<DoctorSchedule>(entity =>
            {
                entity.ToTable("DoctorSchedules");
                entity.HasKey(ds => ds.Id);
                entity.Property(ds => ds.Date).IsRequired();
                entity.Property(ds => ds.Shift).IsRequired();
                entity.Property(ds => ds.StartTime).IsRequired();
                entity.Property(ds => ds.EndTime).IsRequired();
                entity.Property(ds => ds.MaxPatients).IsRequired();
                entity.Property(ds => ds.CurrentBookings).HasDefaultValue(0);

                entity.HasOne(ds => ds.Doctor)
                      .WithMany()
                      .HasForeignKey(ds => ds.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Appointment entity
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointments");
                entity.HasKey(a => a.Id);
                entity.Property(a => a.AppointmentCode).IsRequired().HasMaxLength(50);
                entity.Property(a => a.PatientName).IsRequired().HasMaxLength(150);
                entity.Property(a => a.PatientPhone).IsRequired().HasMaxLength(20);
                entity.Property(a => a.PatientEmail).HasMaxLength(100);
                entity.Property(a => a.AppointmentDate).IsRequired();
                entity.Property(a => a.TimeSlot).IsRequired();
                entity.Property(a => a.Status).IsRequired();
                entity.Property(a => a.Notes).HasMaxLength(500);
                entity.Property(a => a.CreatedAt).IsRequired();

                entity.HasOne(a => a.Doctor)
                      .WithMany()
                      .HasForeignKey(a => a.DoctorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Schedule)
                      .WithMany()
                      .HasForeignKey(a => a.ScheduleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure ReceptionQueue entity
            modelBuilder.Entity<ReceptionQueue>(entity =>
            {
                entity.ToTable("ReceptionQueues");
                entity.HasKey(rq => rq.Id);
                entity.Property(rq => rq.Date).IsRequired();
                entity.Property(rq => rq.QueueNumber).IsRequired();
                entity.Property(rq => rq.Status).IsRequired();
                entity.Property(rq => rq.CheckInTime).IsRequired();

                entity.HasOne(rq => rq.Appointment)
                      .WithMany()
                      .HasForeignKey(rq => rq.AppointmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
