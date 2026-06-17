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
    public class SchedulesController : ControllerBase
    {
        private readonly IRepository<DoctorSchedule> _scheduleRepository;
        private readonly IRepository<Doctor> _doctorRepository;

        public SchedulesController(
            IRepository<DoctorSchedule> scheduleRepository,
            IRepository<Doctor> doctorRepository)
        {
            _scheduleRepository = scheduleRepository ?? throw new ArgumentNullException(nameof(scheduleRepository));
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        }

        // GET: api/schedules
        [HttpGet]
        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedules(
            [FromQuery] Guid? doctorId,
            [FromQuery] DateTime? date)
        {
            try
            {
                var query = _scheduleRepository.GetQueryable()
                    .Include(s => s.Doctor)
                    .AsQueryable();

                if (doctorId.HasValue)
                {
                    query = query.Where(s => s.DoctorId == doctorId.Value);
                }

                if (date.HasValue)
                {
                    var targetDate = date.Value.Date;
                    query = query.Where(s => s.Date == targetDate);
                }

                var schedules = await query.ToListAsync();

                var scheduleDtos = schedules.Select(s => new ScheduleDto
                {
                    Id = s.Id,
                    DoctorId = s.DoctorId,
                    DoctorName = s.Doctor != null ? s.Doctor.FullName : "Khong xac dinh",
                    Date = s.Date,
                    Shift = s.Shift.ToString(),
                    StartTime = s.StartTime.ToString(@"hh\:mm\:ss"),
                    EndTime = s.EndTime.ToString(@"hh\:mm\:ss"),
                    MaxPatients = s.MaxPatients,
                    CurrentBookings = s.CurrentBookings
                }).ToList();

                return Ok(scheduleDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // GET: api/schedules/available
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetAvailableSchedules(
            [FromQuery] string? specialty,
            [FromQuery] Guid? doctorId)
        {
            try
            {
                var today = DateTime.Today;

                var query = _scheduleRepository.GetQueryable()
                    .Include(s => s.Doctor)
                    .Where(s => s.Date >= today && s.CurrentBookings < s.MaxPatients && s.Doctor != null && s.Doctor.IsActive)
                    .AsQueryable();

                if (doctorId.HasValue)
                {
                    query = query.Where(s => s.DoctorId == doctorId.Value);
                }

                if (!string.IsNullOrWhiteSpace(specialty))
                {
                    query = query.Where(s => s.Doctor!.Specialty.ToLower().Contains(specialty.ToLower()));
                }

                var schedules = await query.OrderBy(s => s.Date).ThenBy(s => s.StartTime).ToListAsync();

                var scheduleDtos = schedules.Select(s => new ScheduleDto
                {
                    Id = s.Id,
                    DoctorId = s.DoctorId,
                    DoctorName = s.Doctor != null ? s.Doctor.FullName : "Khong xac dinh",
                    Date = s.Date,
                    Shift = s.Shift.ToString(),
                    StartTime = s.StartTime.ToString(@"hh\:mm\:ss"),
                    EndTime = s.EndTime.ToString(@"hh\:mm\:ss"),
                    MaxPatients = s.MaxPatients,
                    CurrentBookings = s.CurrentBookings
                }).ToList();

                return Ok(scheduleDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // POST: api/schedules
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ScheduleDto>> CreateSchedule([FromBody] CreateScheduleDto createScheduleDto)
        {
            try
            {
                if (createScheduleDto == null)
                {
                    return BadRequest("Du lieu khong hop le.");
                }

                var doctor = await _doctorRepository.GetByIdAsync(createScheduleDto.DoctorId);
                if (doctor == null || !doctor.IsActive)
                {
                    return BadRequest("Bac si khong ton tai hoac da ngung hoat dong.");
                }

                if (!TimeSpan.TryParse(createScheduleDto.StartTime, out var startTime))
                {
                    return BadRequest("Dinh dang gio bat dau khong hop le (hh:mm:ss).");
                }

                if (!TimeSpan.TryParse(createScheduleDto.EndTime, out var endTime))
                {
                    return BadRequest("Dinh dang gio ket thuc khong hop le (hh:mm:ss).");
                }

                if (endTime <= startTime)
                {
                    return BadRequest("Gio ket thuc phai sau gio bat dau.");
                }

                if (createScheduleDto.MaxPatients <= 0)
                {
                    return BadRequest("So luong benh nhan toi da phai lon hon 0.");
                }

                if (createScheduleDto.Shift < 1 || createScheduleDto.Shift > 3)
                {
                    return BadRequest("Ca lam viec khong hop le. 1: Sang, 2: Chieu, 3: Toi.");
                }

                // Check duplicate schedule for the same doctor, date, and shift
                var existingSchedule = await _scheduleRepository.GetQueryable()
                    .FirstOrDefaultAsync(s => s.DoctorId == createScheduleDto.DoctorId && 
                                             s.Date == createScheduleDto.Date.Date && 
                                             s.Shift == (ShiftType)createScheduleDto.Shift);

                if (existingSchedule != null)
                {
                    return BadRequest("Lich lam viec cho bac si vao ca nay va ngay nay da ton tai.");
                }

                var schedule = new DoctorSchedule
                {
                    Id = Guid.NewGuid(),
                    DoctorId = createScheduleDto.DoctorId,
                    Date = createScheduleDto.Date.Date,
                    Shift = (ShiftType)createScheduleDto.Shift,
                    StartTime = startTime,
                    EndTime = endTime,
                    MaxPatients = createScheduleDto.MaxPatients,
                    CurrentBookings = 0
                };

                await _scheduleRepository.AddAsync(schedule);
                var result = await _scheduleRepository.SaveAsync();

                if (!result)
                {
                    return StatusCode(500, "Khong the luu lich lam viec vao co so du lieu.");
                }

                var scheduleDto = new ScheduleDto
                {
                    Id = schedule.Id,
                    DoctorId = schedule.DoctorId,
                    DoctorName = doctor.FullName,
                    Date = schedule.Date,
                    Shift = schedule.Shift.ToString(),
                    StartTime = schedule.StartTime.ToString(@"hh\:mm\:ss"),
                    EndTime = schedule.EndTime.ToString(@"hh\:mm\:ss"),
                    MaxPatients = schedule.MaxPatients,
                    CurrentBookings = schedule.CurrentBookings
                };

                return Ok(scheduleDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }
    }
}
