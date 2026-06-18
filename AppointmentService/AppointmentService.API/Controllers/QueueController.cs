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
    public class QueueController : ControllerBase
    {
        private readonly IRepository<ReceptionQueue> _queueRepository;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<DoctorSchedule> _scheduleRepository;

        public QueueController(
            IRepository<ReceptionQueue> queueRepository,
            IRepository<Appointment> appointmentRepository,
            IRepository<DoctorSchedule> scheduleRepository)
        {
            _queueRepository = queueRepository ?? throw new ArgumentNullException(nameof(queueRepository));
            _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
            _scheduleRepository = scheduleRepository ?? throw new ArgumentNullException(nameof(scheduleRepository));
        }

        // POST: api/queue/confirm-appointment/{appointmentId}
        [HttpPost("confirm-appointment/{appointmentId}")]
        [Authorize(Roles = "Admin,Receptionist,Nurse")]
        public async Task<ActionResult<AppointmentDto>> ConfirmAppointment(Guid appointmentId)
        {
            try
            {
                // 1. Get Appointment
                var appointment = await _appointmentRepository.GetQueryable()
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.Id == appointmentId);

                if (appointment == null)
                {
                    return NotFound($"Khong tim thay lich hen voi Id: {appointmentId}");
                }

                if (appointment.Status != AppointmentStatus.ChoXacNhan)
                {
                    return BadRequest($"Khong the xac nhan lich hen dang o trang thai: {appointment.Status}");
                }

                // 2. Get Schedule
                var schedule = await _scheduleRepository.GetByIdAsync(appointment.ScheduleId);
                if (schedule == null)
                {
                    return BadRequest("Lich lam viec cua bac si khong ton tai.");
                }

                // Double check if bookings exceeded
                if (schedule.CurrentBookings >= schedule.MaxPatients)
                {
                    return BadRequest("Lich lam viec cua bac si da day benh nhan, khong the xac nhan them.");
                }

                // 3. Generate sequential queue number for the Doctor on that specific Date
                var targetDate = appointment.AppointmentDate.Date;
                var doctorId = appointment.DoctorId;

                var existingQueueCount = await _queueRepository.GetQueryable()
                    .Include(q => q.Appointment)
                    .CountAsync(q => q.Date == targetDate && q.Appointment!.DoctorId == doctorId);

                int nextQueueNumber = existingQueueCount + 1;

                // 4. Update Appointment
                appointment.Status = AppointmentStatus.DaXacNhan;
                appointment.QueueNumber = nextQueueNumber;
                _appointmentRepository.Update(appointment);

                // 5. Update DoctorSchedule bookings count
                schedule.CurrentBookings += 1;
                _scheduleRepository.Update(schedule);

                // 6. Create ReceptionQueue item
                var queueItem = new ReceptionQueue
                {
                    Id = Guid.NewGuid(),
                    AppointmentId = appointment.Id,
                    Date = targetDate,
                    QueueNumber = nextQueueNumber,
                    Status = QueueStatus.ChoKham,
                    CheckInTime = DateTime.UtcNow
                };

                await _queueRepository.AddAsync(queueItem);

                // 7. Save database
                var saved = await _appointmentRepository.SaveAsync();
                if (!saved)
                {
                    return StatusCode(500, "Khong the luu thong tin xac nhan lich hen va xep hang.");
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
                    DoctorName = appointment.Doctor != null ? appointment.Doctor.FullName : "Khong xac dinh",
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

        // GET: api/queue/active
        [HttpGet("active")]
        [AllowAnonymous] // TV displays, public, or patients checking status
        public async Task<ActionResult<IEnumerable<QueueDto>>> GetActiveQueue([FromQuery] Guid? doctorId)
        {
            try
            {
                var today = DateTime.Today;

                var query = _queueRepository.GetQueryable()
                    .Include(q => q.Appointment)
                    .ThenInclude(a => a!.Doctor)
                    .Where(q => q.Date == today && (q.Status == QueueStatus.ChoKham || q.Status == QueueStatus.DangKham))
                    .AsQueryable();

                if (doctorId.HasValue)
                {
                    query = query.Where(q => q.Appointment!.DoctorId == doctorId.Value);
                }

                var queueItems = await query.OrderBy(q => q.QueueNumber).ToListAsync();

                var dtos = queueItems.Select(q => new QueueDto
                {
                    Id = q.Id,
                    AppointmentId = q.AppointmentId,
                    PatientName = q.Appointment != null ? q.Appointment.PatientName : "An danh",
                    DoctorName = (q.Appointment != null && q.Appointment.Doctor != null) ? q.Appointment.Doctor.FullName : "Khong xac dinh",
                    Date = q.Date,
                    QueueNumber = q.QueueNumber,
                    Status = q.Status.ToString(),
                    CheckInTime = q.CheckInTime
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // PUT: api/queue/{id}/status
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Receptionist,Nurse,Doctor")]
        public async Task<IActionResult> UpdateQueueStatus(Guid id, [FromBody] UpdateQueueStatusDto statusDto)
        {
            try
            {
                if (statusDto == null)
                {
                    return BadRequest("Du lieu khong hop le.");
                }

                var queueItem = await _queueRepository.GetQueryable()
                    .Include(q => q.Appointment)
                    .FirstOrDefaultAsync(q => q.Id == id);

                if (queueItem == null)
                {
                    return NotFound($"Khong tim thay hang cho voi Id: {id}");
                }

                if (statusDto.Status < 1 || statusDto.Status > 4)
                {
                    return BadRequest("Trang thai hang cho khong hop le. 1: ChoKham, 2: DangKham, 3: BoQua, 4: DaHoanThanh.");
                }

                var targetStatus = (QueueStatus)statusDto.Status;
                queueItem.Status = targetStatus;

                // Sync status with Appointment
                if (queueItem.Appointment != null)
                {
                    if (targetStatus == QueueStatus.DangKham)
                    {
                        queueItem.Appointment.Status = AppointmentStatus.DangKham;
                    }
                    else if (targetStatus == QueueStatus.DaHoanThanh)
                    {
                        queueItem.Appointment.Status = AppointmentStatus.DaKham;
                    }
                    else if (targetStatus == QueueStatus.BoQua)
                    {
                        queueItem.Appointment.Status = AppointmentStatus.DaHuy; // Or keep DaXacNhan but cancel from queue. In this case, we can set status to DaHuy
                    }
                    _appointmentRepository.Update(queueItem.Appointment);
                }

                _queueRepository.Update(queueItem);
                var saved = await _queueRepository.SaveAsync();

                if (!saved)
                {
                    return StatusCode(500, "Khong the cap nhat trang thai hang cho.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }
    }
}
