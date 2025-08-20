using System.Net.Http.Json;
using WMS.Application.DTOs.ReceiptDocument.Response;
using WMS.UI.Models.ReceiptDocument;
using WMS.UI.Models.ReceiptDocument.Request;
using static System.Net.WebRequestMethods;

namespace WMS.UI.Services
{
    public class ReceiptsService
    {
        private readonly HttpClient _httpClient;
        
        public ReceiptsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool Success, string? Error)> CreateAsync(ReceiptDocumentParamsRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/receipts", request);

            if (response.IsSuccessStatusCode)
                return (true, null);

            var errors = await response.Content.ReadFromJsonAsync<List<string>>();
            return (false, errors != null && errors.Any() ? string.Join("; ", errors) : "Неизвестная ошибка");
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

        public async Task<(bool Success, string? Error)> UpdateAsync(Guid id, ReceiptDocumentParamsRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/v1/receipts/{id}", request);

            if (response.IsSuccessStatusCode)
                return (true, null);

            var errors = await response.Content.ReadFromJsonAsync<List<string>>();
            return (false, errors != null && errors.Any() ? string.Join("; ", errors) : "Неизвестная ошибка");
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
            List<Guid>? resourceIds = null, // Изменили на List<Guid>
            List<Guid>? unitOfMeasureIds = null) // Изменили на List<Guid>
        {
            try
            {
                var queryParams = new List<string>();

                if (startDate.HasValue)
                    queryParams.Add($"startDate={Uri.EscapeDataString(startDate.Value.ToUniversalTime().ToString("O"))}");

                if (endDate.HasValue)
                    queryParams.Add($"endDate={Uri.EscapeDataString(endDate.Value.ToUniversalTime().ToString("O"))}");

                if (!string.IsNullOrEmpty(applicationNumberFilter))
                    queryParams.Add($"applicationNumberFilter={Uri.EscapeDataString(applicationNumberFilter)}");

                // Добавляем ID ресурсов (через запятую)
                if (resourceIds != null && resourceIds.Any())
                    queryParams.Add($"resourceIds={string.Join(",", resourceIds)}");

                // Добавляем ID единиц измерения (через запятую)
                if (unitOfMeasureIds != null && unitOfMeasureIds.Any())
                    queryParams.Add($"unitOfMeasureIds={string.Join(",", unitOfMeasureIds)}");

                var url = "api/v1/receipts";
                if (queryParams.Count > 0)
                    url += "?" + string.Join("&", queryParams);

                return await _httpClient.GetFromJsonAsync<List<ReceiptDocumentInfoResponse>>(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}