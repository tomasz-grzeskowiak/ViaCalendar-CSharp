namespace WebAPI.Controllers;
using APIContracts.DTOs.Event;
using Entities;
using APIContracts;
using Microsoft.AspNetCore.Mvc;
using Services.Event;

[ApiController]
[Route("event")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    //Service used in constructor
    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    // GET /eventEntity
    [HttpGet]
    public async Task<ActionResult> GetCompanies()
    {
        var eventsList = _eventService.GetManyAsync();
        var eventsDto = eventsList
            .Select(e => new EventDto( e.Name))
            .ToList();

        return Ok(eventsDto);
    }

    // POST /eventEntity
    [HttpPost]
    public async Task<ActionResult> CreateEvent([FromBody] CreateEventDto dto)
    {
        var eventEntity = new Event.Builder()
            .SetId(dto.Id)
            .SetName(dto.Name)
            .Build();

        await _eventService.CreateAsync(eventEntity);
        return CreatedAtAction(nameof(CreateEvent), new { nameof=dto.Name }, dto);
    }

    // PUT /eventEntity
    [HttpPut]
    public async Task<ActionResult> UpdateEvent([FromBody] CreateEventDto dto)
    {
        var existing = await _eventService.GetSingleAsync(dto.Id);
        if (existing == null)
            return NotFound("Event not found");

        var eventEntity = new Event.Builder()
            .SetId(existing.Id)
            .SetName(dto.Name)
            .Build();

        await _eventService.UpdateAsync(eventEntity);
        return NoContent();
    }
}