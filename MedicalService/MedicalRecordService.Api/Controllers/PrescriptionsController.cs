using MedicalRecordService.Api.Services.Interfaces;
using MedicalRecordService.Models.DTOs.Prescription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        /// <summary>
        /// Lấy chi tiết đơn thuốc theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIdAsync(id);
            if (prescription == null)
                return NotFound($"Không tìm thấy đơn thuốc với ID: {id}");

            return Ok(prescription);
        }

        /// <summary>
        /// Lấy đơn thuốc theo hồ sơ bệnh án
        /// </summary>
        [HttpGet("medicalrecord/{medicalRecordId}")]
        public async Task<IActionResult> GetByMedicalRecordId(Guid medicalRecordId)
        {
            var prescription = await _prescriptionService.GetPrescriptionByMedicalRecordIdAsync(medicalRecordId);
            if (prescription == null)
                return NotFound($"Không tìm thấy đơn thuốc cho hồ sơ với ID: {medicalRecordId}");

            return Ok(prescription);
        }

        /// <summary>
        /// Tạo mới đơn thuốc (kèm theo danh sách thuốc)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePrescriptionDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _prescriptionService.CreatePrescriptionAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }


    }
}