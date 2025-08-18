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
    
    public async Task<bool> CreateAsync(ResourceParamsRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/resources", request);
        return response.IsSuccessStatusCode;
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
    
    public async Task<List<ResourceResponse>> GetByStateAsync(State state)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<List<ResourceResponse>>($"api/v1/resources?state={state}") 
                ?? new List<ResourceResponse>();
        }
        catch
        {
            return new List<ResourceResponse>();
        }
    }
}