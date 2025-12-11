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
    public async Task UpdateAsync(int id, CreateEventDto dto)
    {
        // Use id in the URL, e.g., PUT /event/{id}
        await _http.PutAsJsonAsync($"event/{id}", dto);
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
    
    public async Task<List<EventDto>> GetEventsAsync()
    {
        var response = await _http.GetFromJsonAsync<List<EventDto>>("api/events");
        return response ?? new List<EventDto>();
    }
}