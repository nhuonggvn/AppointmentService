using AutoMapper;
using MassTransit;
using MedicalRecordService.Api.Events;
using MedicalRecordService.Api.Services.Interfaces;
using MedicalRecordService.Data.Repositories;
using MedicalRecordService.Models.DTOs.Prescription;
using MedicalRecordService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Services.Implementations
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IRepository<Prescription> _prescriptionRepo;
        private readonly IRepository<MedicalRecord> _medicalRecordRepo;
        private readonly IRepository<PrescriptionDetail> _detailRepo;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public PrescriptionService(
            IRepository<Prescription> prescriptionRepo,
            IRepository<MedicalRecord> medicalRecordRepo,
            IRepository<PrescriptionDetail> detailRepo,
            IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            _prescriptionRepo = prescriptionRepo;
            _medicalRecordRepo = medicalRecordRepo;
            _detailRepo = detailRepo;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<PrescriptionDto?> GetPrescriptionByIdAsync(Guid id)
        {
            var prescription = await _prescriptionRepo.GetByIdAsync(id);
            if (prescription == null) return null;

            // Load details
            var details = await _detailRepo.FindAsync(d => d.PrescriptionId == id);
            prescription.Details = details.ToList();

            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<PrescriptionDto?> GetPrescriptionByMedicalRecordIdAsync(Guid medicalRecordId)
        {
            var prescriptions = await _prescriptionRepo.FindAsync(p => p.MedicalRecordId == medicalRecordId);
            var prescription = prescriptions.FirstOrDefault();
            if (prescription == null) return null;

            var details = await _detailRepo.FindAsync(d => d.PrescriptionId == prescription.Id);
            prescription.Details = details.ToList();

            return _mapper.Map<PrescriptionDto>(prescription);
        }

        public async Task<PrescriptionDto> CreatePrescriptionAsync(CreatePrescriptionDto createDto)
        {
            // Kiểm tra MedicalRecord tồn tại
            var medicalRecord = await _medicalRecordRepo.GetByIdAsync(createDto.MedicalRecordId);
            if (medicalRecord == null)
                throw new InvalidOperationException("Hồ sơ bệnh án không tồn tại.");

            // Kiểm tra đã có đơn thuốc chưa (chỉ 1 đơn/ hồ sơ)
            var existing = await _prescriptionRepo.FindAsync(p => p.MedicalRecordId == createDto.MedicalRecordId);
            if (existing.Any())
                throw new InvalidOperationException("Hồ sơ này đã có đơn thuốc.");

            var prescription = new Prescription
            {
                Id = Guid.NewGuid(),
                MedicalRecordId = createDto.MedicalRecordId,
                CreatedAt = DateTime.UtcNow,
                Notes = createDto.Notes
            };

            // Tạo chi tiết đơn
            var details = new List<PrescriptionDetail>();
            foreach (var detailDto in createDto.Details)
            {
                details.Add(new PrescriptionDetail
                {
                    Id = Guid.NewGuid(),
                    PrescriptionId = prescription.Id,
                    MedicineId = detailDto.MedicineId,
                    Quantity = detailDto.Quantity,
                    DosageInstruction = detailDto.DosageInstruction
                });
            }

            // Lưu vào DB
            await _prescriptionRepo.AddAsync(prescription);
            foreach (var detail in details)
                await _detailRepo.AddAsync(detail);
            await _prescriptionRepo.SaveChangesAsync(); // Save all

            // Publish event để các Service khác (ví dụ Pharmacy Service) biết
            await _publishEndpoint.Publish(new PrescriptionCreatedEvent(
                prescription.Id,
                prescription.MedicalRecordId,
                details.Select(d => new PrescriptionMedicineItem(d.MedicineId, d.Quantity)).ToList(),
                prescription.CreatedAt
            ));

            // Gán details để map sang DTO
            prescription.Details = details;
            return _mapper.Map<PrescriptionDto>(prescription);
        }
    }
}