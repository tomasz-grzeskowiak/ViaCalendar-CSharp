using System.Net.Http;
using System.Net.Http.Json;
using APIContracts.DTOs.Calendar;

namespace ViaCalendarApp.Services;

public class CalendarServiceClient
{
    private readonly HttpClient _http;

    public CalendarServiceClient(HttpClient http)
    {
        _http = http;
    }

    // Get all companies
    public async Task<List<CalendarDto>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<CalendarDto>>("calendar") ?? new List<CalendarDto>();
    }

    // Create a event
    public async Task CreateAsync(CreateCalendarDto calendarDto)
    {
        var response = await _http.PostAsJsonAsync("calendar", calendarDto);
        response.EnsureSuccessStatusCode();
    }

    // Update an event
    public async Task UpdateAsync(CreateCalendarDto calendarDto)
    {
        var response = await _http.PutAsJsonAsync("calendar", calendarDto);
        response.EnsureSuccessStatusCode();
    }
    public async Task DeleteAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"calendar/{id}");
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
    public async Task GetSingleAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"calendar/{id}");
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}