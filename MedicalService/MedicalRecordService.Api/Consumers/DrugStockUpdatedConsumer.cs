using MassTransit;
using MedicalRecordService.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Consumers
{
    /// <summary>
    /// Consumer lắng nghe sự kiện từ Pharmacy Service khi tồn kho thuốc thay đổi
    /// Có thể dùng để cập nhật trạng thái đơn thuốc hoặc gửi thông báo
    /// </summary>
    public class DrugStockUpdatedConsumer : IConsumer<DrugStockUpdatedEvent>
    {
        private readonly ILogger<DrugStockUpdatedConsumer> _logger;
        private readonly IPrescriptionService _prescriptionService;

        public DrugStockUpdatedConsumer(
            ILogger<DrugStockUpdatedConsumer> logger,
            IPrescriptionService prescriptionService)
        {
            _logger = logger;
            _prescriptionService = prescriptionService;
        }

        public async Task Consume(ConsumeContext<DrugStockUpdatedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation("Received DrugStockUpdatedEvent for DrugId: {DrugId}, NewStock: {NewStock}", 
                message.DrugId, message.NewStock);

            // Xử lý nghiệp vụ nếu cần:
            // 1. Kiểm tra xem có đơn thuốc nào đang chờ thuốc này không?
            // 2. Nếu tồn kho thấp, gửi cảnh báo
            // 3. Cập nhật trạng thái đơn thuốc nếu thuốc đã được xuất

            // Ví dụ: Nếu tồn kho = 0, có thể đánh dấu đơn thuốc liên quan là Cancelled
            // Tuy nhiên, logic này có thể đặt ở Pharmacy Service.

            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Event từ Pharmacy Service báo cập nhật tồn kho thuốc
    /// </summary>
    public class DrugStockUpdatedEvent
    {
        public Guid DrugId { get; set; }
        public string DrugName { get; set; } = string.Empty;
        public int NewStock { get; set; }
        public int OldStock { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}