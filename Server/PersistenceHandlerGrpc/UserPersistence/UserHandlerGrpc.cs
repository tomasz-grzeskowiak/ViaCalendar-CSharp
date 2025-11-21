using System.ComponentModel;
using APIContracts.DTOs;
using APIContracts.ENUMs;
using Entities;
using GrpcAPI.Services;
using PersistenceContracts;
namespace PersistenceHandlerGrpc.UserPersistence;

public class UserHandlerGrpc : ICalendarPersistenceHandler
{
    private readonly UserServiceProto _userService ;
    public UserHandlerGrpc(UserServiceProto _userService)
    {
        this._userService = _userService;
    }

    public async Task<object> HandleAsync(Request request)
    {
        var userEntity = (User)request.Payload?? throw new ArgumentNullException(nameof(request.Payload));
        switch (request.Action)
        {
            case ActionType.ActionCreate:
            {
                return await _userService.CreateAsync(userEntity);
            }
            case ActionType.ActionUpdate:
            {
                await _userService.UpdateAsync(userEntity);
                break;
            }
            case ActionType.ActionDelete:
            {
                await _userService.DeleteAsync(userEntity.Id);
                break;
            }
            case ActionType.ActionGet:
            {
                return await _userService.GetSingleAsync(userEntity.Id);
            }
            case ActionType.ActionList:
            {
                return _userService.GetManyAsync();
            }
            default:
                throw new InvalidEnumArgumentException("Unknown action type");
        }
        return Task.CompletedTask;
    }
}