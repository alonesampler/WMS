using System.Net.Http.Json;
using WMS.Application.DTOs.ReceiptDocument.Response;

namespace WMS.UI.Services
{
    public class ReceiptsService
    {
        private readonly HttpClient _http;

        public ReceiptsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ReceiptDocumentInfoResponse>?> GetAllWithFiltersAsync(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? applicationNumberFilter = null,
            string? resourceTitleFilter = null,
            string? unitOfMeasureTitleFilter = null)
        {
            // собираем query string
            var queryParams = new List<string>();
            if (startDate.HasValue) queryParams.Add($"startDate={startDate.Value:yyyy-MM-dd}");
            if (endDate.HasValue) queryParams.Add($"endDate={endDate.Value:yyyy-MM-dd}");
            if (!string.IsNullOrWhiteSpace(applicationNumberFilter)) queryParams.Add($"applicationNumberFilter={applicationNumberFilter}");
            if (!string.IsNullOrWhiteSpace(resourceTitleFilter)) queryParams.Add($"resourceTitleFilter={resourceTitleFilter}");
            if (!string.IsNullOrWhiteSpace(unitOfMeasureTitleFilter)) queryParams.Add($"unitOfMeasureTitleFilter={unitOfMeasureTitleFilter}");

            var url = "api/receipts";
            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            return await _http.GetFromJsonAsync<List<ReceiptDocumentInfoResponse>>(url);
        }
    }
}