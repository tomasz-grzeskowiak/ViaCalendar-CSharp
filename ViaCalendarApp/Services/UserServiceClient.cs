using System.Net.Http;
using System.Net.Http.Json;
using APIContracts.DTOs.User;

namespace ViaCalendarApp.Services;

public class UserServiceClient
{
    private readonly HttpClient _http;

    public UserServiceClient(HttpClient http)
    {
        _http = http;
    }

    // Get all companies
    public async Task<List<UsertDto>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<UsertDto>>("user") ?? new List<UsertDto>();
    }

    // Create a user
    public async Task CreateAsync(CreateUserDto userDto)
    {
        var response = await _http.PostAsJsonAsync("user", userDto);
        response.EnsureSuccessStatusCode();
    }

    // Update a user
    public async Task UpdateAsync(CreateUserDto userDto)
    {
        var response = await _http.PutAsJsonAsync("user", userDto);
        response.EnsureSuccessStatusCode();
    }
    public async Task DeleteAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"user/{id}");
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
    public async Task GetSingleAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"user/{id}");
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}