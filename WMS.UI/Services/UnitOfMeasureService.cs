using System.Net.Http.Json;
using WMS.UI.Models.Enums;
using WMS.UI.Models.Resource.Response;
using WMS.UI.Models.UnitOfMeasure;
using WMS.UI.Models.UnitOfMeasure.Request;
using WMS.UI.Models.UnitOfMeasure.Response;

namespace BlazorClient.Services;

public class UnitOfMeasureService
{
    private readonly HttpClient _httpClient;
    
    public UnitOfMeasureService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<UnitOfMeasure> GetByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<UnitOfMeasure>($"api/v1/unit-of-measures/{id}");
    }
    
    public async Task<bool> CreateAsync(UnitOfMeasureParamsRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/unit-of-measures", request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> UpdateAsync(Guid id, UnitOfMeasureParamsRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/v1/unit-of-measures/{id}", request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/v1/unit-of-measures/{id}");
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> ArchiveAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/v1/unit-of-measures/{id}/archive", null);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> RestoreAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/v1/unit-of-measures/{id}/restore", null);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<List<UnitOfMeasureResponse>> GetByStateAsync(State state, string? search = null)
    {
        try
        {
            var url = $"api/v1/unit-of-measures?state={state}";
        
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={Uri.EscapeDataString(search)}";

            return await _httpClient.GetFromJsonAsync<List<UnitOfMeasureResponse>>(url) 
                   ?? new List<UnitOfMeasureResponse>();
        }
        catch
        {
            return new List<UnitOfMeasureResponse>();
        }
    }
}