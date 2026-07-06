using MedicalRecordService.Models.DTOs.MedicalRecord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Services.Interfaces
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecordDto>> GetAllMedicalRecordsAsync();
        Task<MedicalRecordDto?> GetMedicalRecordByIdAsync(Guid id);
        Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsByPatientAsync(Guid patientId);
        Task<MedicalRecordDto> CreateMedicalRecordAsync(CreateMedicalRecordDto createDto);
        Task<bool> UpdateMedicalRecordAsync(Guid id, UpdateMedicalRecordDto updateDto);
        Task<bool> DeleteMedicalRecordAsync(Guid id);
    }
}