using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using AppointmentService.API.Controllers;
using AppointmentService.API.DTOs;
using AppointmentService.Domain.Entities;
using AppointmentService.Domain.Enums;
using AppointmentService.Infrastructure.Data;
using AppointmentService.Infrastructure.Repositories.Implementations;

namespace AppointmentService.Tests
{
    public class AppointmentServiceTests
    {
        private AppointmentDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppointmentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Moi test case dung 1 DB rieng biet
                .Options;

            var context = new AppointmentDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task BookAppointment_Success_WhenDataIsValid()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var doctorRepo = new Repository<Doctor>(context);
            var scheduleRepo = new Repository<DoctorSchedule>(context);
            var appointmentRepo = new Repository<Appointment>(context);

            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                FullName = "Nguyen Van An",
                Specialty = "Noi khoa",
                IsActive = true
            };
            await doctorRepo.AddAsync(doctor);

            var schedule = new DoctorSchedule
            {
                Id = Guid.NewGuid(),
                DoctorId = doctor.Id,
                Date = DateTime.Today,
                Shift = ShiftType.Sang,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(11, 30, 0),
                MaxPatients = 5,
                CurrentBookings = 0
            };
            await scheduleRepo.AddAsync(schedule);
            await context.SaveChangesAsync();

            var controller = new AppointmentsController(appointmentRepo, scheduleRepo, doctorRepo);

            var bookDto = new BookAppointmentDto
            {
                PatientName = "Tran Van B",
                PatientPhone = "0987654321",
                PatientEmail = "b.tran@gmail.com",
                AppointmentDate = DateTime.Today,
                TimeSlot = "09:00:00",
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id,
                Notes = "Kham ho hap"
            };

            // Act
            var actionResult = await controller.BookAppointment(bookDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedDto = Assert.IsType<AppointmentDto>(okResult.Value);
            
            Assert.NotNull(returnedDto);
            Assert.Equal("Tran Van B", returnedDto.PatientName);
            Assert.Equal(AppointmentStatus.ChoXacNhan.ToString(), returnedDto.Status);
            Assert.StartsWith("LH", returnedDto.AppointmentCode);

            // Verify db count
            var appointmentsInDb = await context.Appointments.ToListAsync();
            Assert.Single(appointmentsInDb);
            Assert.Equal("09:00:00", appointmentsInDb[0].TimeSlot.ToString(@"hh\:mm\:ss"));
        }

        [Fact]
        public async Task BookAppointment_Fail_WhenScheduleIsFull()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var doctorRepo = new Repository<Doctor>(context);
            var scheduleRepo = new Repository<DoctorSchedule>(context);
            var appointmentRepo = new Repository<Appointment>(context);

            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                FullName = "Nguyen Van An",
                Specialty = "Noi khoa",
                IsActive = true
            };
            await doctorRepo.AddAsync(doctor);

            var schedule = new DoctorSchedule
            {
                Id = Guid.NewGuid(),
                DoctorId = doctor.Id,
                Date = DateTime.Today,
                Shift = ShiftType.Sang,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(11, 30, 0),
                MaxPatients = 1, // Gioi han 1 benh nhan
                CurrentBookings = 0
            };
            await scheduleRepo.AddAsync(schedule);

            // Seed 1 active appointment to fill the schedule
            var existingAppointment = new Appointment
            {
                Id = Guid.NewGuid(),
                AppointmentCode = "LH111111",
                PatientName = "Benh nhan X",
                PatientPhone = "0999999999",
                AppointmentDate = DateTime.Today,
                TimeSlot = new TimeSpan(8, 30, 0),
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id,
                Status = AppointmentStatus.ChoXacNhan
            };
            await appointmentRepo.AddAsync(existingAppointment);
            await context.SaveChangesAsync();

            var controller = new AppointmentsController(appointmentRepo, scheduleRepo, doctorRepo);

            var bookDto = new BookAppointmentDto
            {
                PatientName = "Tran Van B",
                PatientPhone = "0987654321",
                PatientEmail = "b.tran@gmail.com",
                AppointmentDate = DateTime.Today,
                TimeSlot = "09:00:00",
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id,
                Notes = "Kham ho hap"
            };

            // Act
            var actionResult = await controller.BookAppointment(bookDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Ca lam viec nay cua bac si da nhan du so luong lich hen.", badRequestResult.Value);
        }

        [Fact]
        public async Task BookAppointment_Fail_WhenDuplicateBookingOnSameDay()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var doctorRepo = new Repository<Doctor>(context);
            var scheduleRepo = new Repository<DoctorSchedule>(context);
            var appointmentRepo = new Repository<Appointment>(context);

            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                FullName = "Nguyen Van An",
                Specialty = "Noi khoa",
                IsActive = true
            };
            await doctorRepo.AddAsync(doctor);

            var schedule = new DoctorSchedule
            {
                Id = Guid.NewGuid(),
                DoctorId = doctor.Id,
                Date = DateTime.Today,
                Shift = ShiftType.Sang,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(11, 30, 0),
                MaxPatients = 5,
                CurrentBookings = 0
            };
            await scheduleRepo.AddAsync(schedule);

            // Seed 1 appointment for Patient Y
            var existingApp = new Appointment
            {
                Id = Guid.NewGuid(),
                AppointmentCode = "LH111111",
                PatientName = "Benh nhan Y",
                PatientPhone = "0987654321", // Cung so dien thoai
                AppointmentDate = DateTime.Today,
                TimeSlot = new TimeSpan(8, 30, 0),
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id,
                Status = AppointmentStatus.ChoXacNhan
            };
            await appointmentRepo.AddAsync(existingApp);
            await context.SaveChangesAsync();

            var controller = new AppointmentsController(appointmentRepo, scheduleRepo, doctorRepo);

            var bookDto = new BookAppointmentDto
            {
                PatientName = "Benh Nhan Y",
                PatientPhone = "0987654321", // Trung SĐT
                AppointmentDate = DateTime.Today,
                TimeSlot = "09:30:00",
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id
            };

            // Act
            var actionResult = await controller.BookAppointment(bookDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Benh nhan da dat mot lich hen khac voi bac si nay trong cung ngay.", badRequestResult.Value);
        }

        [Fact]
        public async Task ConfirmAppointment_Success_GeneratesSequentialQueueNumber()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var queueRepo = new Repository<ReceptionQueue>(context);
            var appointmentRepo = new Repository<Appointment>(context);
            var scheduleRepo = new Repository<DoctorSchedule>(context);
            var doctorRepo = new Repository<Doctor>(context);

            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                FullName = "Nguyen Van An",
                Specialty = "Noi khoa",
                IsActive = true
            };
            await doctorRepo.AddAsync(doctor);

            var schedule = new DoctorSchedule
            {
                Id = Guid.NewGuid(),
                DoctorId = doctor.Id,
                Date = DateTime.Today,
                Shift = ShiftType.Sang,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(11, 30, 0),
                MaxPatients = 10,
                CurrentBookings = 0
            };
            await scheduleRepo.AddAsync(schedule);

            // Active queue number 1 already exists for this doctor on this day
            var app1 = new Appointment
            {
                Id = Guid.NewGuid(),
                AppointmentCode = "LH111111",
                PatientName = "Benh nhan 1",
                PatientPhone = "0900000001",
                AppointmentDate = DateTime.Today,
                TimeSlot = new TimeSpan(8, 30, 0),
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id,
                Status = AppointmentStatus.DaXacNhan,
                QueueNumber = 1
            };
            await appointmentRepo.AddAsync(app1);

            var queue1 = new ReceptionQueue
            {
                Id = Guid.NewGuid(),
                AppointmentId = app1.Id,
                Date = DateTime.Today,
                QueueNumber = 1,
                Status = QueueStatus.ChoKham,
                CheckInTime = DateTime.UtcNow
            };
            await queueRepo.AddAsync(queue1);
            schedule.CurrentBookings = 1;

            // Pending appointment that we want to confirm
            var app2 = new Appointment
            {
                Id = Guid.NewGuid(),
                AppointmentCode = "LH222222",
                PatientName = "Benh nhan 2",
                PatientPhone = "0900000002",
                AppointmentDate = DateTime.Today,
                TimeSlot = new TimeSpan(9, 0, 0),
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id,
                Status = AppointmentStatus.ChoXacNhan
            };
            await appointmentRepo.AddAsync(app2);
            await context.SaveChangesAsync();

            var controller = new QueueController(queueRepo, appointmentRepo, scheduleRepo);

            // Act
            var actionResult = await controller.ConfirmAppointment(app2.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedDto = Assert.IsType<AppointmentDto>(okResult.Value);

            Assert.NotNull(returnedDto);
            Assert.Equal(AppointmentStatus.DaXacNhan.ToString(), returnedDto.Status);
            Assert.Equal(2, returnedDto.QueueNumber); // Should be number 2

            // Verify Db updates
            var updatedApp2 = await appointmentRepo.GetByIdAsync(app2.Id);
            Assert.NotNull(updatedApp2);
            Assert.Equal(AppointmentStatus.DaXacNhan, updatedApp2.Status);
            Assert.Equal(2, updatedApp2.QueueNumber);

            var updatedSchedule = await scheduleRepo.GetByIdAsync(schedule.Id);
            Assert.NotNull(updatedSchedule);
            Assert.Equal(2, updatedSchedule.CurrentBookings); // Incremented from 1 to 2

            var queuesInDb = await context.ReceptionQueues.ToListAsync();
            Assert.Equal(2, queuesInDb.Count);
            var newQueue = queuesInDb.FirstOrDefault(q => q.AppointmentId == app2.Id);
            Assert.NotNull(newQueue);
            Assert.Equal(2, newQueue.QueueNumber);
            Assert.Equal(QueueStatus.ChoKham, newQueue.Status);
        }

        [Fact]
        public async Task BookAppointment_Fail_WhenSlotIsConflicted()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var doctorRepo = new Repository<Doctor>(context);
            var scheduleRepo = new Repository<DoctorSchedule>(context);
            var appointmentRepo = new Repository<Appointment>(context);

            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                FullName = "Nguyen Van An",
                Specialty = "Noi khoa",
                IsActive = true
            };
            await doctorRepo.AddAsync(doctor);

            var schedule = new DoctorSchedule
            {
                Id = Guid.NewGuid(),
                DoctorId = doctor.Id,
                Date = DateTime.Today,
                Shift = ShiftType.Sang,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(11, 30, 0),
                MaxPatients = 5,
                CurrentBookings = 1
            };
            await scheduleRepo.AddAsync(schedule);

            // Gieo giac mot lich hen ton tai luc 09:00:00 cua bac si nay
            var existingAppointment = new Appointment
            {
                Id = Guid.NewGuid(),
                AppointmentCode = "LH111111",
                PatientName = "Benh nhan X",
                PatientPhone = "0999999999",
                AppointmentDate = DateTime.Today,
                TimeSlot = new TimeSpan(9, 0, 0),
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id,
                Status = AppointmentStatus.ChoXacNhan
            };
            await appointmentRepo.AddAsync(existingAppointment);
            await context.SaveChangesAsync();

            var controller = new AppointmentsController(appointmentRepo, scheduleRepo, doctorRepo);

            // Benh nhan Y yeu cau dat lich cung bac si, cung ngay, cung khung gio 09:00:00
            var bookDto = new BookAppointmentDto
            {
                PatientName = "Benh nhan Y",
                PatientPhone = "0987654321",
                PatientEmail = "y.benhnhan@gmail.com",
                AppointmentDate = DateTime.Today,
                TimeSlot = "09:00:00",
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id,
                Notes = "Kham tim mach"
            };

            // Act
            var actionResult = await controller.BookAppointment(bookDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal("Khung gio hen nay cua bac si da co benh nhan khac dang ky kham.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetBookedSlots_ReturnCorrectSlots()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var doctorRepo = new Repository<Doctor>(context);
            var scheduleRepo = new Repository<DoctorSchedule>(context);
            var appointmentRepo = new Repository<Appointment>(context);

            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                FullName = "Nguyen Van An",
                Specialty = "Noi khoa",
                IsActive = true
            };
            await doctorRepo.AddAsync(doctor);

            var schedule = new DoctorSchedule
            {
                Id = Guid.NewGuid(),
                DoctorId = doctor.Id,
                Date = DateTime.Today,
                Shift = ShiftType.Sang,
                StartTime = new TimeSpan(8, 0, 0),
                EndTime = new TimeSpan(11, 30, 0),
                MaxPatients = 5,
                CurrentBookings = 2
            };
            await scheduleRepo.AddAsync(schedule);

            // Gieo giac hai lich hen (mot lich hoat dong va mot lich bi huy)
            var appointment1 = new Appointment
            {
                Id = Guid.NewGuid(),
                AppointmentCode = "LH111111",
                PatientName = "Benh nhan X",
                PatientPhone = "0999999999",
                AppointmentDate = DateTime.Today,
                TimeSlot = new TimeSpan(9, 0, 0),
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id,
                Status = AppointmentStatus.ChoXacNhan
            };
            var appointment2 = new Appointment
            {
                Id = Guid.NewGuid(),
                AppointmentCode = "LH222222",
                PatientName = "Benh nhan Y",
                PatientPhone = "0888888888",
                AppointmentDate = DateTime.Today,
                TimeSlot = new TimeSpan(10, 0, 0),
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id,
                Status = AppointmentStatus.DaHuy // Bi huy, khong duoc tra ve trong booked-slots
            };

            await appointmentRepo.AddAsync(appointment1);
            await appointmentRepo.AddAsync(appointment2);
            await context.SaveChangesAsync();

            var controller = new AppointmentsController(appointmentRepo, scheduleRepo, doctorRepo);

            // Act
            var actionResult = await controller.GetBookedSlots(doctor.Id, DateTime.Today);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var slots = Assert.IsType<List<string>>(okResult.Value);
            
            Assert.Single(slots);
            Assert.Contains("09:00:00", slots);
            Assert.DoesNotContain("10:00:00", slots);
        }
    }
}
