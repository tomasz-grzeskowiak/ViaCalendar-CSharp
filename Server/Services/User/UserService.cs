using APIContracts.DTOs;
using APIContracts.ENUMs;
using Microsoft.Extensions.DependencyInjection;
using PersistenceContracts;

namespace Services.User;
using Entities;
public class UserService : IUserService
{
    private readonly ICalendarPersistenceHandler _handler;

    public UserService([FromKeyedServices("user")] ICalendarPersistenceHandler handler)
    {
        _handler = handler;
    }
    public async Task<User> CreateAsync(User payload)
    {
        //Logic here
        var request = MakeUserRequest(ActionType.ActionCreate, payload);
        return (User)await _handler.HandleAsync(request);
    }

    public async Task UpdateAsync(User payload)
    {
        //Logic here
        var request = MakeUserRequest(ActionType.ActionUpdate, payload);
        await _handler.HandleAsync(request);
    }

    public async Task DeleteAsync(int id)
    {
        //Logic here
        var request = MakeUserRequest(ActionType.ActionDelete,
            new User.Builder()
                .SetId(id)
                .Build());
        await _handler.HandleAsync(request);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        //Logic here
        var request = MakeUserRequest(ActionType.ActionGet,
            new User.Builder()
                .SetId(id)
                .Build());
        return (User)await _handler.HandleAsync(request);
    }

    public IQueryable<User> GetManyAsync()
    {
        Request request = MakeUserRequest(ActionType.ActionList, new User.Builder()
            .SetId(0)
            .Build());

        var result = _handler.HandleAsync(request).Result as IQueryable<User> ;
        if (result != null)
        {
            return result;
        }
        throw new InvalidOperationException("No users found");
    }
    
    private Request MakeUserRequest(ActionType action, User userEntity)
    {
        return new Request(HandlerType.HandlerUser, action, userEntity);
    }
}