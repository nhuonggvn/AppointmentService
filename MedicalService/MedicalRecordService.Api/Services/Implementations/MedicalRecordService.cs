using AutoMapper;
using MedicalRecordService.Api.Services.Interfaces;
using MedicalRecordService.Data.Repositories;
using MedicalRecordService.Models.DTOs.MedicalRecord;
using MedicalRecordService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Services.Implementations
{
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IRepository<MedicalRecord> _recordRepo;
        private readonly IRepository<PatientProfile> _patientRepo;
        private readonly IMapper _mapper;

        public MedicalRecordService(
            IRepository<MedicalRecord> recordRepo,
            IRepository<PatientProfile> patientRepo,
            IMapper mapper)
        {
            _recordRepo = recordRepo;
            _patientRepo = patientRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetAllMedicalRecordsAsync()
        {
            var records = await _recordRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<MedicalRecordDto>>(records);
        }

        public async Task<MedicalRecordDto?> GetMedicalRecordByIdAsync(Guid id)
        {
            var record = await _recordRepo.GetByIdAsync(id);
            return record == null ? null : _mapper.Map<MedicalRecordDto>(record);
        }

        public async Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByPatientAsync(Guid patientId)
        {
            var records = await _recordRepo.FindAsync(r => r.PatientProfileId == patientId);
            return _mapper.Map<IEnumerable<MedicalRecordDto>>(records);
        }

        public async Task<MedicalRecordDto> CreateMedicalRecordAsync(CreateMedicalRecordDto createDto)
        {
            // Kiểm tra patient profile tồn tại
            var patient = await _patientRepo.GetByIdAsync(createDto.PatientProfileId);
            if (patient == null)
                throw new InvalidOperationException("Hồ sơ bệnh nhân không tồn tại.");

            var record = _mapper.Map<MedicalRecord>(createDto);
            record.Id = Guid.NewGuid();

            await _recordRepo.AddAsync(record);
            await _recordRepo.SaveChangesAsync();

            return _mapper.Map<MedicalRecordDto>(record);
        }

        public async Task<bool> UpdateMedicalRecordAsync(Guid id, UpdateMedicalRecordDto updateDto)
        {
            var record = await _recordRepo.GetByIdAsync(id);
            if (record == null) return false;

            _mapper.Map(updateDto, record);
            _recordRepo.Update(record);
            await _recordRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMedicalRecordAsync(Guid id)
        {
            var record = await _recordRepo.GetByIdAsync(id);
            if (record == null) return false;

            _recordRepo.Delete(record);
            await _recordRepo.SaveChangesAsync();
            return true;
        }
    }
}