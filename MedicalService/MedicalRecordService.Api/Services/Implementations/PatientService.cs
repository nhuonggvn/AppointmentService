using AutoMapper;
using MedicalRecordService.Api.Services.Interfaces;
using MedicalRecordService.Data.Repositories;
using MedicalRecordService.Models.DTOs.Patient;
using MedicalRecordService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<PatientProfile> _patientRepo;
        private readonly IMapper _mapper;

        public PatientService(IRepository<PatientProfile> patientRepo, IMapper mapper)
        {
            _patientRepo = patientRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _patientRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto?> GetPatientByIdAsync(Guid id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            return patient == null ? null : _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto createDto)
        {
            // Kiểm tra trùng số điện thoại
            var existing = await _patientRepo.FindAsync(p => p.PhoneNumber == createDto.PhoneNumber);
            if (existing.Any())
                throw new InvalidOperationException("Số điện thoại đã tồn tại.");

            var patient = _mapper.Map<PatientProfile>(createDto);
            patient.Id = Guid.NewGuid();

            await _patientRepo.AddAsync(patient);
            await _patientRepo.SaveChangesAsync();

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<bool> UpdatePatientAsync(Guid id, UpdatePatientDto updateDto)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            if (patient == null) return false;

            _mapper.Map(updateDto, patient);
            _patientRepo.Update(patient);
            await _patientRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePatientAsync(Guid id)
        {
            var patient = await _patientRepo.GetByIdAsync(id);
            if (patient == null) return false;

            _patientRepo.Delete(patient);
            await _patientRepo.SaveChangesAsync();
            return true;
        }

        public async Task<PatientDto?> SearchPatientAsync(string? phone)
        {
            if (string.IsNullOrEmpty(phone))
                return null;

            var list = await _patientRepo.FindAsync(p => p.PhoneNumber == phone);
            var patient = list.FirstOrDefault();

            return patient == null ? null : _mapper.Map<PatientDto>(patient);
        }
    }
}