using MassTransit;
using MedicalRecordService.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MedicalRecordService.Api.Consumers
{
    /// <summary>
    /// Lắng nghe sự kiện khi Pharmacy Service đã xuất thuốc thành công
    /// </summary>
    public class PrescriptionDispensedConsumer : IConsumer<PrescriptionDispensedEvent>
    {
        private readonly ILogger<PrescriptionDispensedConsumer> _logger;
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionDispensedConsumer(
            ILogger<PrescriptionDispensedConsumer> logger,
            IPrescriptionService prescriptionService)
        {
            _logger = logger;
            _prescriptionService = prescriptionService;
        }

        public async Task Consume(ConsumeContext<PrescriptionDispensedEvent> context)
        {
            var message = context.Message;
            _logger.LogInformation(
                "Prescription {PrescriptionId} has been dispensed at {DispensedAt}",
                message.PrescriptionId,
                message.DispensedAt
            );

            // Do đơn thuốc không còn lưu trữ trường Status, chỉ ghi nhận log thông tin
            await Task.CompletedTask;
        }
    }

    public class PrescriptionDispensedEvent
    {
        public Guid PrescriptionId { get; set; }
        public DateTime DispensedAt { get; set; }
    }
}