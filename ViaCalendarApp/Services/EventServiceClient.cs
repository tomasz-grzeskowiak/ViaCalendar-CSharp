using System.Net.Http;
using System.Net.Http.Json;
using APIContracts.DTOs.Event;

namespace ViaCalendarApp.Services;

public class EventServiceClient
{
    private readonly HttpClient _http;

    public EventServiceClient(HttpClient http)
    {
        _http = http;
    }

    // Get all companies
    public async Task<List<EventDto>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<EventDto>>("event") ?? new List<EventDto>();
    }

    // Create a event
    public async Task CreateAsync(CreateEventDto eventDto)
    {
        var response = await _http.PostAsJsonAsync("event", eventDto);
        response.EnsureSuccessStatusCode();
    }

    // Update an event
    public async Task UpdateAsync(CreateEventDto eventDto)
    {
        var response = await _http.PutAsJsonAsync("event", eventDto);
        response.EnsureSuccessStatusCode();
    }
    public async Task DeleteAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"event/{id}");
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
    public async Task GetSingleAsync(int id)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"event/{id}");
        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}