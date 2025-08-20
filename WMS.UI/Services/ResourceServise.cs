using System.Net.Http.Json;
using WMS.UI.Models.Enums;
using WMS.UI.Models.Resource;
using WMS.UI.Models.Resource.Request;
using WMS.UI.Models.Resource.Response;

namespace BlazorClient.Services;

public class ResourceService
{
    private readonly HttpClient _httpClient;
    
    public ResourceService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<Resource> GetByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<Resource>($"api/v1/resources/{id}");
    }
    
    public async Task<(bool Success, string ErrorMessage)> CreateAsync(ResourceParamsRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/v1/resources", request);
        
            if (response.IsSuccessStatusCode)
                return (true, null);
            
            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, $"Ошибка: {response.StatusCode}. {errorContent}");
        }
        catch (Exception ex)
        {
            return (false, $"Ошибка при создании: {ex.Message}");
        }
    }
    
    public async Task<bool> UpdateAsync(Guid id, ResourceParamsRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/v1/resources/{id}", request);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/v1/resources/{id}");
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> ArchiveAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/v1/resources/{id}/archive", null);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<bool> RestoreAsync(Guid id)
    {
        var response = await _httpClient.PostAsync($"api/v1/resources/{id}/restore", null);
        return response.IsSuccessStatusCode;
    }
    
    public async Task<List<ResourceResponse>> GetByStateAsync(State state, string? search = null)
    {
        try
        {
            var url = $"api/v1/resources?state={state}";
        
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={Uri.EscapeDataString(search)}";

            return await _httpClient.GetFromJsonAsync<List<ResourceResponse>>(url) 
                   ?? new List<ResourceResponse>();
        }
        catch
        {
            return new List<ResourceResponse>();
        }
    }
}