using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedicalRecordService.Data.Repositories
{
    /// <summary>
    /// Generic repository interface cho các thao tác CRUD cơ bản
    /// </summary>
    /// <typeparam name="T">Kiểu Entity</typeparam>
    public interface IRepository<T> where T : class
    {
        // Lấy một entity theo Id (Guid)
        Task<T?> GetByIdAsync(Guid id);

        // Lấy tất cả entity
        Task<IEnumerable<T>> GetAllAsync();

        // Tìm kiếm theo điều kiện (LINQ Expression)
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Thêm mới entity
        Task AddAsync(T entity);

        // Cập nhật entity (đã tracking)
        void Update(T entity);

        // Xóa entity
        void Delete(T entity);

        // Lưu thay đổi xuống database (thường dùng khi có nhiều thay đổi)
        Task SaveChangesAsync();
    }
}