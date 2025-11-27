using System.ComponentModel;
using APIContracts.DTOs;
using APIContracts.ENUMs;
using Entities;
using GrpcAPI.Services;
using PersistenceContracts;
namespace PersistenceHandlerGrpc.GroupPersistence;

public class GroupHandlerGrpc : ICalendarPersistenceHandler
{
    private readonly GroupServiceProto _groupService ;
    public GroupHandlerGrpc(GroupServiceProto _groupService)
    {
        this._groupService = _groupService;
    }

    public async Task<object> HandleAsync(Request request)
    {
        var groupEntity = (Group)request.Payload?? throw new ArgumentNullException(nameof(request.Payload));
        switch (request.Action)
        {
            case ActionType.ActionCreate:
            {
                return await _groupService.CreateAsync(groupEntity);
            }
            case ActionType.ActionUpdate:
            {
                await _groupService.UpdateAsync(groupEntity);
                break;
            }
            case ActionType.ActionDelete:
            {
                await _groupService.DeleteAsync(groupEntity.Id);
                break;
            }
            case ActionType.ActionGet:
            {
                return await _groupService.GetSingleAsync(groupEntity.Id);
            }
            case ActionType.ActionList:
            {
                return _groupService.GetManyAsync();
            }
            default:
                throw new InvalidEnumArgumentException("Unknown action type");
        }
        return Task.CompletedTask;
    }
}