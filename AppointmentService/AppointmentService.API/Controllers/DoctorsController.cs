using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppointmentService.Domain.Entities;
using AppointmentService.Infrastructure.Repositories.Interfaces;
using AppointmentService.API.DTOs;

namespace AppointmentService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IRepository<Doctor> _doctorRepository;

        public DoctorsController(IRepository<Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository ?? throw new ArgumentNullException(nameof(doctorRepository));
        }

        // GET: api/doctors
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors([FromQuery] string? specialty, [FromQuery] bool? isActive)
        {
            try
            {
                var query = _doctorRepository.GetQueryable();

                if (!string.IsNullOrWhiteSpace(specialty))
                {
                    query = query.Where(d => d.Specialty.ToLower().Contains(specialty.ToLower()));
                }

                if (isActive.HasValue)
                {
                    query = query.Where(d => d.IsActive == isActive.Value);
                }
                else
                {
                    // Default return only active doctors for public view
                    query = query.Where(d => d.IsActive);
                }

                var doctors = await query.ToListAsync();

                var doctorDtos = doctors.Select(d => new DoctorDto
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    Specialty = d.Specialty,
                    Qualifications = d.Qualifications,
                    ConsultationFee = d.ConsultationFee,
                    IsActive = d.IsActive
                }).ToList();

                return Ok(doctorDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // GET: api/doctors/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<DoctorDto>> GetDoctor(Guid id)
        {
            try
            {
                var doctor = await _doctorRepository.GetByIdAsync(id);

                if (doctor == null)
                {
                    return NotFound($"Khong tim thay bac si voi Id: {id}");
                }

                var doctorDto = new DoctorDto
                {
                    Id = doctor.Id,
                    FullName = doctor.FullName,
                    Specialty = doctor.Specialty,
                    Qualifications = doctor.Qualifications,
                    ConsultationFee = doctor.ConsultationFee,
                    IsActive = doctor.IsActive
                };

                return Ok(doctorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // POST: api/doctors
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DoctorDto>> CreateDoctor([FromBody] CreateDoctorDto createDoctorDto)
        {
            try
            {
                if (createDoctorDto == null)
                {
                    return BadRequest("Du lieu khong hop le.");
                }

                if (string.IsNullOrWhiteSpace(createDoctorDto.FullName))
                {
                    return BadRequest("Ho ten bac si khong duoc de trong.");
                }

                if (string.IsNullOrWhiteSpace(createDoctorDto.Specialty))
                {
                    return BadRequest("Chuyen khoa khong duoc de trong.");
                }

                var doctor = new Doctor
                {
                    Id = Guid.NewGuid(),
                    FullName = createDoctorDto.FullName,
                    Specialty = createDoctorDto.Specialty,
                    Qualifications = createDoctorDto.Qualifications,
                    ConsultationFee = createDoctorDto.ConsultationFee,
                    IsActive = true
                };

                await _doctorRepository.AddAsync(doctor);
                var result = await _doctorRepository.SaveAsync();

                if (!result)
                {
                    return StatusCode(500, "Khong the luu thong tin bac si vao co so du lieu.");
                }

                var doctorDto = new DoctorDto
                {
                    Id = doctor.Id,
                    FullName = doctor.FullName,
                    Specialty = doctor.Specialty,
                    Qualifications = doctor.Qualifications,
                    ConsultationFee = doctor.ConsultationFee,
                    IsActive = doctor.IsActive
                };

                return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // PUT: api/doctors/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctor(Guid id, [FromBody] CreateDoctorDto updateDoctorDto)
        {
            try
            {
                if (updateDoctorDto == null)
                {
                    return BadRequest("Du lieu khong hop le.");
                }

                var doctor = await _doctorRepository.GetByIdAsync(id);

                if (doctor == null)
                {
                    return NotFound($"Khong tim thay bac si voi Id: {id}");
                }

                if (string.IsNullOrWhiteSpace(updateDoctorDto.FullName))
                {
                    return BadRequest("Ho ten bac si khong duoc de trong.");
                }

                if (string.IsNullOrWhiteSpace(updateDoctorDto.Specialty))
                {
                    return BadRequest("Chuyen khoa khong duoc de trong.");
                }

                doctor.FullName = updateDoctorDto.FullName;
                doctor.Specialty = updateDoctorDto.Specialty;
                doctor.Qualifications = updateDoctorDto.Qualifications;
                doctor.ConsultationFee = updateDoctorDto.ConsultationFee;

                _doctorRepository.Update(doctor);
                var result = await _doctorRepository.SaveAsync();

                if (!result)
                {
                    return StatusCode(500, "Khong the cap nhat thong tin bac si.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Loi he thong: {ex.Message}");
            }
        }

        // DELETE: api/doctors/{id} (Soft delete - Deactivate)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateDoctor(Guid id)
        {
            try
            {
                var doctor = await _doctorRepository.GetByIdAsync(id);

                if (doctor == null)
                {
                    return NotFound($"Khong tim thay bac si voi Id: {id}");
                }

                doctor.IsActive = false; // Soft delete
                _doctorRepository.Update(doctor);
                var result = await _doctorRepository.SaveAsync();

                if (!result)
                {
                    return StatusCode(500, "Khong the tam ngung hoat dong cua bac si.");
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
