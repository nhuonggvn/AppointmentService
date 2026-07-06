using MedicalRecordService.Api.Services.Interfaces;
using MedicalRecordService.Models.DTOs.Patient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Yêu cầu JWT token
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        /// <summary>
        /// Lấy danh sách tất cả bệnh nhân
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        /// <summary>
        /// Lấy chi tiết một bệnh nhân theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
                return NotFound($"Không tìm thấy bệnh nhân với ID: {id}");

            return Ok(patient);
        }

        /// <summary>
        /// Tạo mới bệnh nhân
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPatient = await _patientService.CreatePatientAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = createdPatient.Id }, createdPatient);
        }

        /// <summary>
        /// Cập nhật thông tin bệnh nhân
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePatientDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _patientService.UpdatePatientAsync(id, updateDto);
            if (!updated)
                return NotFound($"Không tìm thấy bệnh nhân với ID: {id}");

            return NoContent(); // 204
        }

        /// <summary>
        /// Xóa bệnh nhân
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _patientService.DeletePatientAsync(id);
            if (!deleted)
                return NotFound($"Không tìm thấy bệnh nhân với ID: {id}");

            return NoContent();
        }

        /// <summary>
        /// Tìm bệnh nhân theo số điện thoại
        /// </summary>
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? phone)
        {
            if (string.IsNullOrEmpty(phone))
                return BadRequest("Vui lòng cung cấp số điện thoại để tìm kiếm.");

            var patient = await _patientService.SearchPatientAsync(phone);
            if (patient == null)
                return NotFound("Không tìm thấy bệnh nhân.");

            return Ok(patient);
        }
    }
}