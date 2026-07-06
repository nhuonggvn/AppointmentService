using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MedicalRecordService.Api.Services.External
{
    public class PharmacyServiceClient : IPharmacyServiceClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PharmacyServiceClient> _logger;

        public PharmacyServiceClient(HttpClient httpClient, ILogger<PharmacyServiceClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<DrugInfoDto?> GetDrugInfoAsync(Guid drugId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/drugs/{drugId}");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to get drug info for {DrugId}. Status: {StatusCode}", drugId, response.StatusCode);
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<DrugInfoDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Pharmacy Service for drug {DrugId}", drugId);
                return null;
            }
        }

        public async Task<bool> CheckStockAsync(Guid drugId, int quantity)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/drugs/{drugId}/stock?quantity={quantity}");
                if (!response.IsSuccessStatusCode)
                    return false;

                var result = await response.Content.ReadFromJsonAsync<StockCheckResult>();
                return result?.IsAvailable ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking stock for drug {DrugId}", drugId);
                return false;
            }
        }

        private class StockCheckResult
        {
            public bool IsAvailable { get; set; }
        }
    }
}