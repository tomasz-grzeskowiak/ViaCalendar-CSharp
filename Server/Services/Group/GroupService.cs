using APIContracts.DTOs;
using APIContracts.ENUMs;
using Microsoft.Extensions.DependencyInjection;
using PersistenceContracts;

namespace Services.Group;
using Entities;
public class GroupService : IGroupService
{
    private readonly ICalendarPersistenceHandler _handler;

    public GroupService([FromKeyedServices("group")] ICalendarPersistenceHandler handler)
    {
        _handler = handler;
    }
    public async Task<Group> CreateAsync(Group payload)
    {
        //Logic here
        var request = MakeGroupRequest(ActionType.ActionCreate, payload);
        return (Group)await _handler.HandleAsync(request);
    }

    public async Task UpdateAsync(Group payload)
    {
        //Logic here
        var request = MakeGroupRequest(ActionType.ActionUpdate, payload);
        await _handler.HandleAsync(request);
    }

    public async Task DeleteAsync(int id)
    {
        //Logic here
        var request = MakeGroupRequest(ActionType.ActionDelete,
            new Group.Builder()
                .SetId(id)
                .Build());
        await _handler.HandleAsync(request);
    }

    public async Task<Group> GetSingleAsync(int id)
    {
        //Logic here
        var request = MakeGroupRequest(ActionType.ActionGet,
            new Group.Builder()
                .SetId(id)
                .Build());
        return (Group)await _handler.HandleAsync(request);
    }

    public IQueryable<Group> GetManyAsync()
    {
        Request request = MakeGroupRequest(ActionType.ActionList, new Group.Builder()
            .SetId(0)
            .Build());

        var result = _handler.HandleAsync(request).Result as IQueryable<Group> ;
        if (result != null)
        {
            return result;
        }
        throw new InvalidOperationException("No groups found");
    }
    
    private Request MakeGroupRequest(ActionType action, Group groupEntity)
    {
        return new Request(HandlerType.HandlerGroup, action, groupEntity);
    }
}