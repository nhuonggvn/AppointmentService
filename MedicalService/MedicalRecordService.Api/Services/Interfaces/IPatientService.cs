using MedicalRecordService.Models.DTOs.Patient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Services.Interfaces
{
    public interface IPatientService
    {
        /// <summary>
        /// Lấy tất cả bệnh nhân
        /// </summary>
        Task<IEnumerable<PatientDto>> GetAllPatientsAsync();

        /// <summary>
        /// Lấy bệnh nhân theo Id
        /// </summary>
        Task<PatientDto?> GetPatientByIdAsync(Guid id);

        /// <summary>
        /// Tìm kiếm bệnh nhân theo số điện thoại
        /// </summary>
        Task<PatientDto?> SearchPatientAsync(string? phone);

        /// <summary>
        /// Tạo mới bệnh nhân
        /// </summary>
        Task<PatientDto> CreatePatientAsync(CreatePatientDto createDto);

        /// <summary>
        /// Cập nhật thông tin bệnh nhân
        /// </summary>
        Task<bool> UpdatePatientAsync(Guid id, UpdatePatientDto updateDto);

        /// <summary>
        /// Xóa bệnh nhân
        /// </summary>
        Task<bool> DeletePatientAsync(Guid id);
    }
}