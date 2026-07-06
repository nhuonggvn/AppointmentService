using MedicalRecordService.Models.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalRecordService.Data.Repositories
{
    public interface IPatientRepository : IRepository<PatientProfile>
    {
        // Tìm bệnh nhân theo số điện thoại
        Task<PatientProfile?> GetByPhoneAsync(string phoneNumber);

        // Lấy danh sách bệnh nhân có phân trang
        Task<(IEnumerable<PatientProfile> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);

        // Lấy bệnh nhân kèm theo danh sách hồ sơ bệnh án (Include)
        Task<PatientProfile?> GetPatientWithRecordsAsync(Guid patientId);
    }
}