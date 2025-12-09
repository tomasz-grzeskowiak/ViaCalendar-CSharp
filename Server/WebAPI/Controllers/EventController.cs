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
            .Select(e => new EventDto(e.Id,e.Name, e.Tag, e.Recursive, e.CreatorId, e.Duration, e.TypeOfRecursive))
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
            .SetRecursive(dto.Recursive)
            .SetTag(dto.Tag)
            .SetCreatorId(dto.CreatorId)
            .SetDuration(dto.Duration)
            .SetTypeOfRecursive(dto.TypeOfRecursive)
            .Build();

        await _eventService.CreateAsync(eventEntity);
        return CreatedAtAction(nameof(CreateEvent), new { nameof=dto.Name, dto.Recursive, dto.Tag, dto.CreatorId, dto.Duration, dto.TypeOfRecursive }, dto);
    }

    // PUT /eventEntity
    [HttpPut]
    public async Task<ActionResult> UpdateEvent([FromBody] CreateEventDto dto)
    {
        var eventEntity = new Event.Builder()
            .SetId(dto.Id)
            .SetName(dto.Name)
            .SetRecursive(dto.Recursive)
            .SetTag(dto.Tag)
            .SetCreatorId(dto.CreatorId)
            .SetDuration(dto.Duration)
            .SetTypeOfRecursive(dto.TypeOfRecursive)
            .Build();

        await _eventService.UpdateAsync(eventEntity);
        return NoContent();
    }
    // GET /event/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CreateEventDto>> GetSingleEventAsync(int id)
    {
        var _event = await _eventService.GetSingleAsync(id);

        return Ok(new CreateEventDto(_event.Id, _event.Name, _event.Tag, _event.Recursive,  _event.CreatorId,  _event.Duration, _event.TypeOfRecursive));
    }
    // DELETE /event/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCompanyAsync(int id)
    {
        await _eventService.DeleteAsync(id);
        return NoContent();
    }
}