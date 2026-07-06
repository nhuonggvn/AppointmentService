using AutoMapper;
using MedicalRecordService.Models.DTOs.Patient;
using MedicalRecordService.Models.DTOs.MedicalRecord;
using MedicalRecordService.Models.DTOs.Prescription;
using MedicalRecordService.Models.Entities;

namespace MedicalRecordService.Api.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // PatientProfile
            CreateMap<PatientProfile, PatientDto>()
                .ForMember(dest => dest.GenderText, opt => opt.MapFrom(src => src.Gender.ToString()));
            CreateMap<CreatePatientDto, PatientProfile>();
            CreateMap<UpdatePatientDto, PatientProfile>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // MedicalRecord
            CreateMap<MedicalRecord, MedicalRecordDto>()
                .ForMember(dest => dest.PatientFullName, opt => opt.MapFrom(src => src.PatientProfile != null ? src.PatientProfile.FullName : string.Empty))
                .ForMember(dest => dest.Prescription, opt => opt.MapFrom(src => src.Prescription));
            CreateMap<CreateMedicalRecordDto, MedicalRecord>();
            CreateMap<UpdateMedicalRecordDto, MedicalRecord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Prescription
            CreateMap<Prescription, PrescriptionDto>()
                .ForMember(dest => dest.Details, opt => opt.MapFrom(src => src.Details));
            CreateMap<PrescriptionDetail, PrescriptionDetailDto>();
            CreateMap<CreatePrescriptionDto, Prescription>();
            CreateMap<CreatePrescriptionDetailDto, PrescriptionDetail>();
        }
    }
}