using MedicalRecordService.Models.DTOs.Prescription;
using System;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Services.Interfaces
{
    public interface IPrescriptionService
    {
        Task<PrescriptionDto?> GetPrescriptionByIdAsync(Guid id);
        Task<PrescriptionDto?> GetPrescriptionByMedicalRecordIdAsync(Guid medicalRecordId);
        Task<PrescriptionDto> CreatePrescriptionAsync(CreatePrescriptionDto createDto);
    }
}