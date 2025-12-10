using System.ComponentModel;
using APIContracts.DTOs;
using APIContracts.ENUMs;
using Entities;
using GrpcAPI.Services;
using PersistenceContracts;
namespace PersistenceHandlerGrpc.CalendarPersistence;

public class CalendarHandlerGrpc : ICalendarPersistenceHandler
{
    private readonly CalendarServiceProto _calendarService ;
    public CalendarHandlerGrpc(CalendarServiceProto _calendarService)
    {
        this._calendarService = _calendarService;
    }

    public async Task<object> HandleAsync(Request request)
    {
        var calendarEntity = (Calendar)request.Payload?? throw new ArgumentNullException(nameof(request.Payload));
        switch (request.Action)
        {
            case ActionType.ActionCreate:
            {
                return await _calendarService.CreateAsync(calendarEntity);
            }
            case ActionType.ActionUpdate:
            {
                await _calendarService.UpdateAsync(calendarEntity);
                break;
            }
            case ActionType.ActionDelete:
            {
                await _calendarService.DeleteAsync(calendarEntity.Id);
                break;
            }
            case ActionType.ActionGet:
            {
                return await _calendarService.GetSingleAsync(calendarEntity.Id);
            }
            case ActionType.ActionList:
            {
                return _calendarService.GetManyAsync();
            }
            default:
                throw new InvalidEnumArgumentException("Unknown action type");
        }
        return Task.CompletedTask;
    }
}