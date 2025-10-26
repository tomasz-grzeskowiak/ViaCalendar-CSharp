using System.ComponentModel;
using APIContracts.DTOs;
using APIContracts.ENUMs;
using Entities;
using GrpcAPI.Services;
using PersistenceContracts;
namespace PersistenceHandlerGrpc.EventPersistence;

public class EventHandlerGrpc : ICalendarPersistenceHandler
{
    private readonly EventServiceProto _eventService ;
    public EventHandlerGrpc(EventServiceProto _eventService)
    {
        this._eventService = _eventService;
    }

    public async Task<object> HandleAsync(Request request)
    {
        var eventEntity = (Event)request.Payload?? throw new ArgumentNullException(nameof(request.Payload));
        switch (request.Action)
        {
            case ActionType.ActionCreate:
            {
                return await _eventService.CreateAsync(eventEntity);
            }
            case ActionType.ActionUpdate:
            {
                await _eventService.UpdateAsync(eventEntity);
                break;
            }
            case ActionType.ActionDelete:
            {
                await _eventService.DeleteAsync(eventEntity.Id);
                break;
            }
            case ActionType.ActionGet:
            {
                return await _eventService.GetSingleAsync(eventEntity.Id);
            }
            case ActionType.ActionList:
            {
                return _eventService.GetManyAsync();
            }
            default:
                throw new InvalidEnumArgumentException("Unknown action type");
        }
        return Task.CompletedTask;
    }
}