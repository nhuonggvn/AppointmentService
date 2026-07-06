using MedicalRecordService.Api.Services.Interfaces;
using MedicalRecordService.Models.DTOs.MedicalRecord;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MedicalRecordsController : ControllerBase
    {
        private readonly IMedicalRecordService _recordService;

        public MedicalRecordsController(IMedicalRecordService recordService)
        {
            _recordService = recordService;
        }

        /// <summary>
        /// Lấy tất cả hồ sơ bệnh án
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _recordService.GetAllMedicalRecordsAsync();
            return Ok(records);
        }

        /// <summary>
        /// Lấy chi tiết một hồ sơ bệnh án theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var record = await _recordService.GetMedicalRecordByIdAsync(id);
            if (record == null)
                return NotFound($"Không tìm thấy hồ sơ với ID: {id}");

            return Ok(record);
        }

        /// <summary>
        /// Lấy danh sách hồ sơ theo bệnh nhân
        /// </summary>
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(Guid patientId)
        {
            var records = await _recordService.GetMedicalRecordsByPatientAsync(patientId);
            return Ok(records);
        }

        /// <summary>
        /// Tạo mới hồ sơ bệnh án
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMedicalRecordDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _recordService.CreateMedicalRecordAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Cập nhật hồ sơ bệnh án
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMedicalRecordDto updateDto) // cần tạo UpdateMedicalRecordDto
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _recordService.UpdateMedicalRecordAsync(id, updateDto);
            if (!updated)
                return NotFound($"Không tìm thấy hồ sơ với ID: {id}");

            return NoContent();
        }

        /// <summary>
        /// Xóa hồ sơ bệnh án
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _recordService.DeleteMedicalRecordAsync(id);
            if (!deleted)
                return NotFound($"Không tìm thấy hồ sơ với ID: {id}");

            return NoContent();
        }
    }
}