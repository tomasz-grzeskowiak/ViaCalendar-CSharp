using System.Net.Http;
using System.Net.Http.Json;
using APIContracts.DTOs.Group;

namespace ViaCalendarApp.Services;

public class GroupServiceClient
{
    private readonly HttpClient _http;

    public GroupServiceClient(HttpClient http)
    {
        _http = http;
    }

    // Get all companies
    public async Task<List<GroupDto>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<GroupDto>>("group") ?? new List<GroupDto>();
    }

    // Create a group
    public async Task CreateAsync(CreateGroupDto groupDto)
    {
        var response = await _http.PostAsJsonAsync("group", groupDto);
        response.EnsureSuccessStatusCode();
    }

    // Update a group
    public async Task UpdateAsync(CreateGroupDto groupDto)
    {
        var response = await _http.PutAsJsonAsync("group", groupDto);
        response.EnsureSuccessStatusCode();
    }
    
    // Delete a group
    public async Task DeleteAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"group/{id}");
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
    public async Task GetSingleAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"group/{id}");
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}