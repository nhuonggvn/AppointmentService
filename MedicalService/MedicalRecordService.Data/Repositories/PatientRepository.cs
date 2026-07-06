using MedicalRecordService.Data.DbContext;
using MedicalRecordService.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalRecordService.Data.Repositories
{
    public class PatientRepository : Repository<PatientProfile>, IPatientRepository
    {
        private readonly MedicalDbContext _context;

        public PatientRepository(MedicalDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PatientProfile?> GetByPhoneAsync(string phoneNumber)
        {
            return await _context.PatientProfiles
                .FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
        }

        public async Task<(IEnumerable<PatientProfile> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.PatientProfiles.AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<PatientProfile?> GetPatientWithRecordsAsync(Guid patientId)
        {
            return await _context.PatientProfiles
                .Include(p => p.MedicalRecords)
                .FirstOrDefaultAsync(p => p.Id == patientId);
        }
    }
}