using APIContracts.DTOs;
using APIContracts.ENUMs;
using Microsoft.Extensions.DependencyInjection;
using PersistenceContracts;

namespace Services.Calendar;
using Entities;
public class CalendarService : ICalendarService
{
    private readonly ICalendarPersistenceHandler _handler;

    public CalendarService([FromKeyedServices("calendar")] ICalendarPersistenceHandler handler)
    {
        _handler = handler;
    }
    public async Task<Calendar> CreateAsync(Calendar payload)
    {
        
        var request = MakeCalendarRequest(ActionType.ActionCreate, payload);
        return (Calendar)await _handler.HandleAsync(request);
    }

    public async Task UpdateAsync(Calendar payload)
    {
        
        var request = MakeCalendarRequest(ActionType.ActionUpdate, payload);
        await _handler.HandleAsync(request);
    }

    public async Task DeleteAsync(int id)
    {
        
        var request = MakeCalendarRequest(ActionType.ActionDelete,
            new Calendar.Builder()
                .SetId(id)
                .Build());
        await _handler.HandleAsync(request);
    }

    public async Task<Calendar> GetSingleAsync(int id)
    {
       
        var request = MakeCalendarRequest(ActionType.ActionGet,
            new Calendar.Builder()
                .SetId(id)
                .Build());
        return (Calendar)await _handler.HandleAsync(request);
    }

    public IQueryable<Calendar> GetManyAsync()
    {
        Request request = MakeCalendarRequest(ActionType.ActionList, new Calendar.Builder()
            .SetId(0)
            .Build());

        var result = _handler.HandleAsync(request).Result as IQueryable<Calendar> ;
        if (result != null)
        {
            return result;
        }
        throw new InvalidOperationException("No calendars found");
    }
    
    private Request MakeCalendarRequest(ActionType action, Calendar calendarEntity)
    {
        return new Request(HandlerType.HandlerCalendar, action, calendarEntity);
    }
}