using MedicalRecordService.Models.DTOs.Patient;
using MedicalRecordService.Models.DTOs.MedicalRecord;
using MedicalRecordService.Models.DTOs.Prescription;
using MedicalRecordService.Models.Entities;
using MedicalRecordService.Models.Enums;
using System;
using System.Linq;

namespace MedicalRecordService.Api.Helpers
{
    public static class MappingHelper
    {
        // ========== PATIENT PROFILE ==========
        public static PatientDto MapToPatientDto(PatientProfile patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                GenderText = patient.Gender.ToString(),
                PhoneNumber = patient.PhoneNumber,
                Address = patient.Address,
                MedicalHistory = patient.MedicalHistory,
                Allergies = patient.Allergies
            };
        }

        public static PatientProfile MapToPatientEntity(CreatePatientDto dto)
        {
            return new PatientProfile
            {
                Id = Guid.NewGuid(),
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                MedicalHistory = dto.MedicalHistory,
                Allergies = dto.Allergies
            };
        }

        // ========== MEDICAL RECORD ==========
        public static MedicalRecordDto MapToMedicalRecordDto(MedicalRecord record)
        {
            return new MedicalRecordDto
            {
                Id = record.Id,
                PatientProfileId = record.PatientProfileId,
                PatientFullName = record.PatientProfile != null ? record.PatientProfile.FullName : string.Empty,
                DoctorId = record.DoctorId,
                AppointmentId = record.AppointmentId,
                ExaminationDate = record.ExaminationDate,
                Symptoms = record.Symptoms,
                Diagnosis = record.Diagnosis,
                DoctorNotes = record.DoctorNotes,
                Prescription = record.Prescription != null ? MapToPrescriptionDto(record.Prescription) : null
            };
        }

        // ========== PRESCRIPTION ==========
        public static PrescriptionDto MapToPrescriptionDto(Prescription prescription)
        {
            return new PrescriptionDto
            {
                Id = prescription.Id,
                MedicalRecordId = prescription.MedicalRecordId,
                CreatedAt = prescription.CreatedAt,
                Notes = prescription.Notes,
                Details = prescription.Details?.Select(d => new PrescriptionDetailDto
                {
                    Id = d.Id,
                    MedicineId = d.MedicineId,
                    Quantity = d.Quantity,
                    DosageInstruction = d.DosageInstruction
                }).ToList() ?? new()
            };
        }
    }
}