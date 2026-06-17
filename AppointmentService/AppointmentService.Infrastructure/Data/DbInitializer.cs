using System;
using System.Collections.Generic;
using System.Linq;
using AppointmentService.Domain.Entities;
using AppointmentService.Domain.Enums;

namespace AppointmentService.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppointmentDbContext context)
        {
            // Create database if it doesn't exist
            context.Database.EnsureCreated();

            // Check if database has been seeded
            if (context.Doctors.Any())
            {
                return; // Database has been seeded
            }

            // Seed Doctors
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    Id = Guid.Parse("d1d11111-1111-1111-1111-111111111111"),
                    FullName = "Nguyen Van An",
                    Specialty = "Noi khoa",
                    Qualifications = "Thac si Baci si",
                    ConsultationFee = 150000,
                    IsActive = true
                },
                new Doctor
                {
                    Id = Guid.Parse("d2d22222-2222-2222-2222-222222222222"),
                    FullName = "Tran Thi Binh",
                    Specialty = "Nhi khoa",
                    Qualifications = "Bac si Chuyen khoa 1",
                    ConsultationFee = 200000,
                    IsActive = true
                },
                new Doctor
                {
                    Id = Guid.Parse("d3d33333-3333-3333-3333-333333333333"),
                    FullName = "Le Hoang Nam",
                    Specialty = "Da lieu",
                    Qualifications = "Bac si Chuyen khoa 2",
                    ConsultationFee = 250000,
                    IsActive = true
                },
                new Doctor
                {
                    Id = Guid.Parse("d4d44444-4444-4444-4444-444444444444"),
                    FullName = "Pham Minh Duc",
                    Specialty = "Rang Ham Mat",
                    Qualifications = "Tien si Y khoa",
                    ConsultationFee = 300000,
                    IsActive = true
                }
            };

            context.Doctors.AddRange(doctors);
            context.SaveChanges();

            // Seed Schedules for the next 7 days starting today
            var schedules = new List<DoctorSchedule>();
            var today = DateTime.Today;

            for (int i = 0; i < 7; i++)
            {
                var targetDate = today.AddDays(i);

                // Doctor 1 (An) - Sang (Mon, Wed, Fri, Sun) or Chieu (Tue, Thu, Sat)
                if (i % 2 == 0)
                {
                    schedules.Add(new DoctorSchedule
                    {
                        Id = Guid.NewGuid(),
                        DoctorId = doctors[0].Id,
                        Date = targetDate,
                        Shift = ShiftType.Sang,
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(11, 30, 0),
                        MaxPatients = 12,
                        CurrentBookings = 0
                    });
                }
                else
                {
                    schedules.Add(new DoctorSchedule
                    {
                        Id = Guid.NewGuid(),
                        DoctorId = doctors[0].Id,
                        Date = targetDate,
                        Shift = ShiftType.Chieu,
                        StartTime = new TimeSpan(13, 30, 0),
                        EndTime = new TimeSpan(17, 0, 0),
                        MaxPatients = 10,
                        CurrentBookings = 0
                    });
                }

                // Doctor 2 (Binh) - Always Chieu
                schedules.Add(new DoctorSchedule
                {
                    Id = Guid.NewGuid(),
                    DoctorId = doctors[1].Id,
                    Date = targetDate,
                    Shift = ShiftType.Chieu,
                    StartTime = new TimeSpan(13, 30, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    MaxPatients = 15,
                    CurrentBookings = 0
                });

                // Doctor 3 (Nam) - Sang or Toi
                if (i % 3 == 0)
                {
                    schedules.Add(new DoctorSchedule
                    {
                        Id = Guid.NewGuid(),
                        DoctorId = doctors[2].Id,
                        Date = targetDate,
                        Shift = ShiftType.Toi,
                        StartTime = new TimeSpan(18, 0, 0),
                        EndTime = new TimeSpan(20, 30, 0),
                        MaxPatients = 8,
                        CurrentBookings = 0
                    });
                }
                else
                {
                    schedules.Add(new DoctorSchedule
                    {
                        Id = Guid.NewGuid(),
                        DoctorId = doctors[2].Id,
                        Date = targetDate,
                        Shift = ShiftType.Sang,
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(11, 30, 0),
                        MaxPatients = 10,
                        CurrentBookings = 0
                    });
                }

                // Doctor 4 (Duc) - Tue, Thu, Sat Sang
                if (i % 2 != 0)
                {
                    schedules.Add(new DoctorSchedule
                    {
                        Id = Guid.NewGuid(),
                        DoctorId = doctors[3].Id,
                        Date = targetDate,
                        Shift = ShiftType.Sang,
                        StartTime = new TimeSpan(8, 0, 0),
                        EndTime = new TimeSpan(11, 30, 0),
                        MaxPatients = 8,
                        CurrentBookings = 0
                    });
                }
            }

            context.DoctorSchedules.AddRange(schedules);
            context.SaveChanges();

            // Seed a few Appointments on Today's Schedules for Doctor 1 (An) and Doctor 2 (Binh)
            var todayAnSchedule = schedules.FirstOrDefault(s => s.DoctorId == doctors[0].Id && s.Date == today);
            var todayBinhSchedule = schedules.FirstOrDefault(s => s.DoctorId == doctors[1].Id && s.Date == today);

            if (todayAnSchedule != null)
            {
                var app1 = new Appointment
                {
                    Id = Guid.NewGuid(),
                    AppointmentCode = "LH" + new Random().Next(100000, 999999).ToString(),
                    PatientName = "Tran Cong Thuong",
                    PatientPhone = "0987654321",
                    PatientEmail = "thuong.tc@spros.com",
                    AppointmentDate = today,
                    TimeSlot = new TimeSpan(9, 0, 0),
                    DoctorId = doctors[0].Id,
                    ScheduleId = todayAnSchedule.Id,
                    Status = AppointmentStatus.ChoXacNhan,
                    QueueNumber = null,
                    Notes = "Dau bung nhe keo dai 2 ngay",
                    CreatedAt = DateTime.UtcNow
                };

                context.Appointments.Add(app1);
            }

            if (todayBinhSchedule != null)
            {
                var app2 = new Appointment
                {
                    Id = Guid.NewGuid(),
                    AppointmentCode = "LH" + new Random().Next(100000, 999999).ToString(),
                    PatientName = "Nguyen Thi Hong Duyen",
                    PatientPhone = "0912345678",
                    PatientEmail = "duyen.nth@spros.com",
                    AppointmentDate = today,
                    TimeSlot = new TimeSpan(14, 0, 0),
                    DoctorId = doctors[1].Id,
                    ScheduleId = todayBinhSchedule.Id,
                    Status = AppointmentStatus.DaXacNhan,
                    QueueNumber = 1,
                    Notes = "Kham dinh ky cho be 3 tuoi",
                    CreatedAt = DateTime.UtcNow
                };

                context.Appointments.Add(app2);

                // Add to Queue
                var queueItem = new ReceptionQueue
                {
                    Id = Guid.NewGuid(),
                    AppointmentId = app2.Id,
                    Date = today,
                    QueueNumber = 1,
                    Status = QueueStatus.ChoKham,
                    CheckInTime = DateTime.UtcNow
                };

                context.ReceptionQueues.Add(queueItem);

                // Update current bookings
                todayBinhSchedule.CurrentBookings = 1;
            }

            context.SaveChanges();
        }
    }
}
