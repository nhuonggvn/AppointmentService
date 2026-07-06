using System;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Services.External
{
    public interface IPharmacyServiceClient
    {
        /// <summary>
        /// Lấy thông tin thuốc từ Pharmacy Service
        /// </summary>
        Task<DrugInfoDto?> GetDrugInfoAsync(Guid drugId);

        /// <summary>
        /// Kiểm tra tồn kho thuốc
        /// </summary>
        Task<bool> CheckStockAsync(Guid drugId, int quantity);
    }

    public class DrugInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }
}