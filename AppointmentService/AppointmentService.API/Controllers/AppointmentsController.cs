using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppointmentService.Domain.Entities;
using AppointmentService.Domain.Enums;
using AppointmentService.Infrastructure.Repositories.Interfaces;
using AppointmentService.API.DTOs;

namespace AppointmentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<DoctorSchedule> _scheduleRepository;
        private readonly IRepository<Doctor> _doctorRepository;

        public AppointmentsController(
            IRepository<Appointment> appointmentRepository,
            IRepository<DoctorSchedule> scheduleRepository,
            IRepository<Doctor> doctorRepository)
        {
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
            _scheduleRepository = scheduleRepository ?? throw new ArgumentNullException(nameof(scheduleRepository));
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        }

        // POST: api/appointments/book
        [HttpPost("book")]
        [AllowAnonymous]
        public async Task<ActionResult<AppointmentDto>> BookAppointment([FromBody] BookAppointmentDto bookDto)
        {
            try
            {
                if (bookDto == null)
                {
                    return BadRequest("Du lieu khong hop le.");
                }

                if (string.IsNullOrWhiteSpace(bookDto.PatientName))
                {
                    return BadRequest("Ho ten benh nhan khong duoc de trong.");
                }

                if (string.IsNullOrWhiteSpace(bookDto.PatientPhone))
                {
                    return BadRequest("So dien thoai benh nhan khong duoc de trong.");
                }

                if (!TimeSpan.TryParse(bookDto.TimeSlot, out var timeSlot))
                {
                    return BadRequest("Dinh dang khung gio khong hop le (hh:mm:ss).");
                }

                // 1. Check if Doctor exists and is active
                var doctor = await _doctorRepository.GetByIdAsync(bookDto.DoctorId);
                if (doctor == null || !doctor.IsActive)
                {
                    return BadRequest("Bac si khong ton tai hoac da ngung hoat dong.");
                }

                // 2. Check if Schedule exists
                var schedule = await _scheduleRepository.GetByIdAsync(bookDto.ScheduleId);
                if (schedule == null)
                {
                    return BadRequest("Lich lam viec cua bac si khong ton tai.");
                }

                if (schedule.DoctorId != bookDto.DoctorId)
                {
                    return BadRequest("Lich lam viec khong khop voi bac si duoc chon.");
                }

                if (schedule.Date.Date != bookDto.AppointmentDate.Date)
                {
                    return BadRequest("Ngay lam viec trong lich khong khop voi ngay hen.");
                }

                // 3. Validate if TimeSlot is between StartTime and EndTime of the schedule
                if (timeSlot < schedule.StartTime || timeSlot > schedule.EndTime)
                {
                    return BadRequest($"Gio hen phai nam trong ca lam viec cua bac si ({schedule.StartTime} - {schedule.EndTime}).");
                }

                // 4. Double booking check (Phone + Doctor + Date)
                var doubleBooking = await _appointmentRepository.GetQueryable()
                    .AnyAsync(a => a.PatientPhone == bookDto.PatientPhone &&
                                   a.DoctorId == bookDto.DoctorId &&
                                   a.AppointmentDate.Date == bookDto.AppointmentDate.Date &&
                                   a.Status != AppointmentStatus.DaHuy);

                if (doubleBooking)
                {
                    return BadRequest("Benh nhan da dat mot lich hen khac voi bac si nay trong cung ngay.");
                }

                // 4.1. Conflict slot check (Same Doctor + Same Date + Same TimeSlot)
                var slotConflicted = await _appointmentRepository.GetQueryable()
                    .AnyAsync(a => a.DoctorId == bookDto.DoctorId &&
                                   a.AppointmentDate.Date == bookDto.AppointmentDate.Date &&
                                   a.TimeSlot == timeSlot &&
                                   a.Status != AppointmentStatus.DaHuy);

                if (slotConflicted)
                {
                    return BadRequest("Khung gio hen nay cua bac si da co benh nhan khac dang ky kham.");
                }

                // 5. Check capacity (Active + Pending appointments count against MaxPatients)
                var activeBookingsCount = await _appointmentRepository.GetQueryable()
                    .CountAsync(a => a.ScheduleId == bookDto.ScheduleId && a.Status != AppointmentStatus.DaHuy);

                if (activeBookingsCount >= schedule.MaxPatients)
                {
                    return BadRequest("Ca lam viec nay cua bac si da nhan du so luong lich hen.");
                }

                // 6. Create Appointment
                var appointment = new Appointment
                {
                    Id = Guid.NewGuid(),
                    AppointmentCode = "LH" + new Random().Next(100000, 999999).ToString(),
                    PatientName = bookDto.PatientName,
                    PatientPhone = bookDto.PatientPhone,
                    PatientEmail = bookDto.PatientEmail,
                    AppointmentDate = bookDto.AppointmentDate.Date,
                    TimeSlot = timeSlot,
                    DoctorId = bookDto.DoctorId,
                    ScheduleId = bookDto.ScheduleId,
                    Status = AppointmentStatus.ChoXacNhan, // Waiting for receptionist
                    QueueNumber = null,
                    Notes = bookDto.Notes,
                    CreatedAt = DateTime.UtcNow
                };

                await _appointmentRepository.AddAsync(appointment);
                var result = await _appointmentRepository.SaveAsync();

                if (!result)
                {
                    return StatusCode(500, "Khong the luu lich hen.");
                }

                var dto = new AppointmentDto
                {
                    Id = appointment.Id,
                    AppointmentCode = appointment.AppointmentCode,
                    PatientName = appointment.PatientName,
                    PatientPhone = appointment.PatientPhone,
                    PatientEmail = appointment.PatientEmail,
                    AppointmentDate = appointment.AppointmentDate,
                    TimeSlot = appointment.TimeSlot.ToString(@"hh\:mm\:ss"),
                    DoctorId = appointment.DoctorId,
                    DoctorName = doctor.FullName,
                    ScheduleId = appointment.ScheduleId,
                    Status = appointment.Status.ToString(),
                    QueueNumber = appointment.QueueNumber,
                    Notes = appointment.Notes,
                    CreatedAt = appointment.CreatedAt
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // GET: api/appointments/pending
        [HttpGet("pending")]
        [Authorize(Roles = "Admin,Receptionist,Nurse")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetPendingAppointments()
        {
            try
            {
                var pendingAppointments = await _appointmentRepository.GetQueryable()
                    .Include(a => a.Doctor)
                    .Where(a => a.Status == AppointmentStatus.ChoXacNhan)
                    .OrderBy(a => a.AppointmentDate)
                    .ThenBy(a => a.TimeSlot)
                    .ToListAsync();

                var dtos = pendingAppointments.Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    AppointmentCode = a.AppointmentCode,
                    PatientName = a.PatientName,
                    PatientPhone = a.PatientPhone,
                    PatientEmail = a.PatientEmail,
                    AppointmentDate = a.AppointmentDate,
                    TimeSlot = a.TimeSlot.ToString(@"hh\:mm\:ss"),
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor != null ? a.Doctor.FullName : "Khong xac dinh",
                    ScheduleId = a.ScheduleId,
                    Status = a.Status.ToString(),
                    QueueNumber = a.QueueNumber,
                    Notes = a.Notes,
                    CreatedAt = a.CreatedAt
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // GET: api/appointments/search
        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> SearchAppointments(
            [FromQuery] string? code, 
            [FromQuery] string? phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(phone))
                {
                    return BadRequest("Vui long cung cap ma lich hen hoac so dien thoai de tim kiem.");
                }

                var query = _appointmentRepository.GetQueryable()
                    .Include(a => a.Doctor)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(code))
                {
                    query = query.Where(a => a.AppointmentCode.ToLower() == code.ToLower());
                }

                if (!string.IsNullOrWhiteSpace(phone))
                {
                    query = query.Where(a => a.PatientPhone == phone);
                }

                var appointments = await query.OrderByDescending(a => a.AppointmentDate).ToListAsync();

                var dtos = appointments.Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    AppointmentCode = a.AppointmentCode,
                    PatientName = a.PatientName,
                    PatientPhone = a.PatientPhone,
                    PatientEmail = a.PatientEmail,
                    AppointmentDate = a.AppointmentDate,
                    TimeSlot = a.TimeSlot.ToString(@"hh\:mm\:ss"),
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor != null ? a.Doctor.FullName : "Khong xac dinh",
                    ScheduleId = a.ScheduleId,
                    Status = a.Status.ToString(),
                    QueueNumber = a.QueueNumber,
                    Notes = a.Notes,
                    CreatedAt = a.CreatedAt
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // PUT: api/appointments/{id}/cancel
        [HttpPut("{id}/cancel")]
        [AllowAnonymous] // Patient can cancel their own, or staff can cancel
        public async Task<IActionResult> CancelAppointment(Guid id)
        {
            try
            {
                var appointment = await _appointmentRepository.GetByIdAsync(id);
                if (appointment == null)
                {
                    return NotFound($"Khong tim thay lich hen voi Id: {id}");
                }

                if (appointment.Status == AppointmentStatus.DaHuy)
                {
                    return BadRequest("Lich hen nay da duoc huy truoc do.");
                }

                if (appointment.Status == AppointmentStatus.DaKham)
                {
                    return BadRequest("Khong the huy lich hen da hoan thanh kham.");
                }

                var oldStatus = appointment.Status;
                appointment.Status = AppointmentStatus.DaHuy;

                _appointmentRepository.Update(appointment);

                // If the appointment was already DaXacNhan, we need to decrement CurrentBookings in the schedule
                if (oldStatus == AppointmentStatus.DaXacNhan)
                {
                    var schedule = await _scheduleRepository.GetByIdAsync(appointment.ScheduleId);
                    if (schedule != null && schedule.CurrentBookings > 0)
                    {
                        schedule.CurrentBookings -= 1;
                        _scheduleRepository.Update(schedule);
                    }
                }

                var result = await _appointmentRepository.SaveAsync();
                if (!result)
                {
                    return StatusCode(500, "Khong the cap nhat trang thai huy lich hen.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // GET: api/appointments/by-phone/{phone}
        [HttpGet("by-phone/{phone}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetByPhone(string phone)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    return BadRequest("So dien thoai khong duoc de trong.");
                }

                var appointments = await _appointmentRepository.GetQueryable()
                    .Include(a => a.Doctor)
                    .Where(a => a.PatientPhone == phone)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ThenByDescending(a => a.TimeSlot)
                    .ToListAsync();

                var dtos = appointments.Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    AppointmentCode = a.AppointmentCode,
                    PatientName = a.PatientName,
                    PatientPhone = a.PatientPhone,
                    PatientEmail = a.PatientEmail,
                    AppointmentDate = a.AppointmentDate,
                    TimeSlot = a.TimeSlot.ToString(@"hh\:mm\:ss"),
                    DoctorId = a.DoctorId,
                    DoctorName = a.Doctor != null ? a.Doctor.FullName : "Khong xac dinh",
                    ScheduleId = a.ScheduleId,
                    Status = a.Status.ToString(),
                    QueueNumber = a.QueueNumber,
                    Notes = a.Notes,
                    CreatedAt = a.CreatedAt
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // GET: api/appointments/booked-slots
        [HttpGet("booked-slots")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<string>>> GetBookedSlots(
            [FromQuery] Guid doctorId,
            [FromQuery] DateTime date)
        {
            try
            {
                var bookedSlots = await _appointmentRepository.GetQueryable()
                    .Where(a => a.DoctorId == doctorId &&
                                a.AppointmentDate.Date == date.Date &&
                                a.Status != AppointmentStatus.DaHuy)
                    .Select(a => a.TimeSlot)
                    .ToListAsync();

                var slotStrings = bookedSlots.Select(ts => ts.ToString(@"hh\:mm\:ss")).ToList();
                return Ok(slotStrings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }
    }
}
