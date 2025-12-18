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
   
    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    // GET 
    [HttpGet]
    public ActionResult GetCompanies()
    {
        var eventsList = _eventService.GetManyAsync().ToList();
        var eventsDto = eventsList
            .Select(e => new EventDto(e.Id, e.Name, e.Tag, e.Recursive, e.CreatorId, e.Start, e.End, e.TypeOfRecursive))
            .ToList();

        return Ok(eventsDto);
    }
    
    // POST 
    [HttpPost]
    public async Task<ActionResult> CreateEvent([FromBody] CreateEventDto dto)
    {
        var eventEntity = new Event.Builder()
            .SetName(dto.Name)
            .SetRecursive(dto.Recursive)
            .SetTag(dto.Tag)
            .SetCreatorId(dto.CreatorId)
            .SetStart(dto.Start)
            .SetEnd(dto.End)
            .SetTypeOfRecursive(dto.TypeOfRecursive)
            .Build();

        await _eventService.CreateAsync(eventEntity);
        return CreatedAtAction(nameof(CreateEvent), new { nameof=dto.Name, dto.Recursive, dto.Tag, dto.CreatorId, dto.Start, dto.End, dto.TypeOfRecursive }, dto);
    }

    // PUT 
    [HttpPut]
    public async Task<ActionResult> UpdateEvent([FromBody] CreateEventDto dto)
    {
        var eventEntity = new Event.Builder()
            .SetName(dto.Name)
            .SetRecursive(dto.Recursive)
            .SetTag(dto.Tag)
            .SetCreatorId(dto.CreatorId)
            .SetStart(dto.Start)
            .SetEnd(dto.End)
            .SetTypeOfRecursive(dto.TypeOfRecursive)
            .Build();

        await _eventService.UpdateAsync(eventEntity);
        return NoContent();
    }
    // GET 
    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetSingleEventAsync(int id)
    {
        var _event = await _eventService.GetSingleAsync(id);

        return Ok(new CreateEventDto(_event.Name, _event.Tag, _event.Recursive, _event.CreatorId, _event.Start, _event.End, _event.TypeOfRecursive));
    }
    // DELETE 
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCompanyAsync(int id)
    {
        await _eventService.DeleteAsync(id);
        return NoContent();
    }
}