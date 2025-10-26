using APIContracts.DTOs;
using APIContracts.ENUMs;
using PersistenceContracts;

namespace Services.Event;
using Entities;
public class EventService : IEventService
{
    private readonly ICalendarPersistenceHandler _handler;

    public EventService(ICalendarPersistenceHandler handler)
    {
        _handler = handler;
    }
    public async Task<Event> CreateAsync(Event payload)
    {
        //Logic here
        var request = MakeEventRequest(ActionType.ActionCreate, payload);
        return (Event)await _handler.HandleAsync(request);
    }

    public async Task UpdateAsync(Event payload)
    {
        //Logic here
        var request = MakeEventRequest(ActionType.ActionCreate, payload);
        await _handler.HandleAsync(request);
    }

    public async Task DeleteAsync(int id)
    {
        //Logic here
        var request = MakeEventRequest(ActionType.ActionCreate,
    new Event.Builder()
            .SetId(id)
            .Build());
         await _handler.HandleAsync(request);
    }

    public async Task<Event> GetSingleAsync(int id)
    {
        //Logic here
        var request = MakeEventRequest(ActionType.ActionCreate,
new Event.Builder()
        .SetId(id)
        .Build());
        return (Event)await _handler.HandleAsync(request);
    }

    public IQueryable<Event> GetManyAsync()
    {
        Request request = MakeEventRequest(ActionType.ActionList, new Event.Builder()
            .SetId(0)
            .Build());

        var result = _handler.HandleAsync(request).Result as IQueryable<Event> ;
        if (result != null)
        {
            return result;
        }
        throw new InvalidOperationException("No events found");
    }
    
    private Request MakeEventRequest(ActionType action, Event eventEntity)
    {
        return new Request(HandlerType.HandlerEvent, action, eventEntity);
    }
}