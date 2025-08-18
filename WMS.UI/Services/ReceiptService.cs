using System.Net.Http.Json;
using WMS.Application.DTOs.ReceiptDocument.Response;
using WMS.UI.Models.ReceiptDocument;
using WMS.UI.Models.ReceiptDocument.Request;

namespace WMS.UI.Services
{
    public class ReceiptsService
    {
        private readonly HttpClient _httpClient;
        
        public ReceiptsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<bool> CreateAsync(ReceiptDocumentParamsRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/receipts", request);
            return response.IsSuccessStatusCode;
        }
        
        public async Task<ReceiptDocument?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ReceiptDocument>($"api/v1/receipts/{id}");
            }
            catch
            {
                return null;
            }
        }
        
        public async Task<bool> UpdateAsync(Guid id, ReceiptDocumentParamsRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/v1/receipts/{id}", request);
            return response.IsSuccessStatusCode;
        }
        
        public async Task<bool> DeleteAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/v1/receipts/{id}");
            return response.IsSuccessStatusCode;
        }
        
        public async Task<List<ReceiptDocumentInfoResponse>?> GetAllWithFiltersAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? applicationNumberFilter = null,
            string? resourceTitleFilter = null,
            string? unitOfMeasureTitleFilter = null)
        {
            try
            {
                var queryParams = new Dictionary<string, string>();
        
                if (startDate.HasValue)
                    queryParams.Add("startDate", startDate.Value.ToUniversalTime().ToString("O"));
        
                if (endDate.HasValue)
                    queryParams.Add("endDate", endDate.Value.ToUniversalTime().ToString("O"));
        
                if (!string.IsNullOrEmpty(applicationNumberFilter))
                    queryParams.Add("applicationNumberFilter", applicationNumberFilter);
        
                if (!string.IsNullOrEmpty(resourceTitleFilter))
                    queryParams.Add("resourceTitleFilter", resourceTitleFilter);
        
                if (!string.IsNullOrEmpty(unitOfMeasureTitleFilter))
                    queryParams.Add("unitOfMeasureTitleFilter", unitOfMeasureTitleFilter);

                var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
                var url = $"api/v1/receipts?{queryString}";

                return await _httpClient.GetFromJsonAsync<List<ReceiptDocumentInfoResponse>>(url);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}