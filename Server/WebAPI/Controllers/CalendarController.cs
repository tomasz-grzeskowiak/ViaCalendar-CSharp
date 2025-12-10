namespace WebAPI.Controllers;
using APIContracts.DTOs.Calendar;
using Entities;
using APIContracts;
using Microsoft.AspNetCore.Mvc;
using Services.Calendar;

[ApiController]
[Route("calendar")]
public class CalendarController : ControllerBase
{
    private readonly ICalendarService _calendarService;
    //Service used in constructor
    public CalendarController(ICalendarService calendarService)
    {
        _calendarService = calendarService;
    }

    // GET /calendarEntity
    [HttpGet]
    public async Task<ActionResult> GetCompanies()
    {
        var calendarsList = _calendarService.GetManyAsync();
        var calendarsDto = calendarsList
            .Select(e => new CalendarDto(e.Id,e.UserId, e.EventIds))
            .ToList();

        return Ok(calendarsDto);
    }

    // POST /eventEntity
    [HttpPost]
    public async Task<ActionResult> CreateCalendar([FromBody] CreateCalendarDto dto)
    {
        var CalendarEntity = new Calendar.Builder()
            .SetId(dto.Id)
            .SetUserId(dto.UserId)
            .SetEventIds(dto.EventIds)
            .Build();

        await _calendarService.CreateAsync(CalendarEntity);
        return CreatedAtAction(nameof(CreateCalendar), new { nameof=dto.UserId, dto.EventIds}, dto);
    }

    // PUT /calendarEntity
    [HttpPut]
    public async Task<ActionResult> UpdateCalendar([FromBody] CreateCalendarDto dto)
    {
        var calendarEntity = new Calendar.Builder()
            .SetId(dto.Id)
            .SetUserId(dto.UserId)
            .SetEventIds(dto.EventIds)
            .Build();

        await _calendarService.UpdateAsync(calendarEntity);
        return NoContent();
    }
    // GET /calendar/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CreateCalendarDto>> GetSingleCalendarAsync(int id)
    {
        var _calendar = await _calendarService.GetSingleAsync(id);

        return Ok(new CreateCalendarDto(_calendar.Id, _calendar.UserId, _calendar.EventIds));
    }
    // DELETE /event/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCompanyAsync(int id)
    {
        await _calendarService.DeleteAsync(id);
        return NoContent();
    }
}