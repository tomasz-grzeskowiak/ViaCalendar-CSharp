using System.Runtime.InteropServices.JavaScript;
using RepositoryContracts;
using Entities;
using Google.Protobuf.WellKnownTypes;
using GrpcAPI.Protos;

namespace GrpcAPI.Services;

public class GroupServiceProto : IGroupRepository
{
    private readonly CalendarMainGrpcHandler _handler;

    public GroupServiceProto(CalendarMainGrpcHandler handler)
    {
        _handler = handler;
    }
    public async Task<Group> CreateAsync(Group payload)
    {
        var proto = new GroupProto()
        {
            Name = payload.Name
        };
        var request = MakeRequestProto(ActionTypeProto.ActionCreate, proto);

        var reponse = await _handler.SendRequestAsync(request);

        var created = reponse.Payload.Unpack<GroupProto>();

        return new Group.Builder()
            .SetName(created.Name)
            .Build();
    }

    public async Task UpdateAsync(Group payload)
    {
        var proto = new GroupProto()
        {
            Id = payload.Id,
            Name = payload.Name
        };
        var update = MakeRequestProto(ActionTypeProto.ActionUpdate, proto);
        await _handler.SendRequestAsync(update);
    }

    public async Task DeleteAsync(int id)
    {
        var proto = new GroupProto()
        {
            Id =id,
        };
        var delete = MakeRequestProto(ActionTypeProto.ActionDelete, proto);
        await _handler.SendRequestAsync(delete);
    }

    public async Task<Group> GetSingleAsync(int id)
    {
        var proto = new GroupProto()
        {
            Id = id,
        };
        var fetched = MakeRequestProto(ActionTypeProto.ActionGet, proto);
        var response = await _handler.SendRequestAsync(fetched);
        var groupProto = response.Payload.Unpack<GroupProto>()
                      ?? throw new InvalidDataException("Group not found");
        return new Group.Builder()
            .SetId(groupProto.Id)
            .SetName(groupProto.Name)
            .Build();
    }

    public IQueryable<Group> GetManyAsync()
    {
            RequestProto request = new()
            {
                Action = ActionTypeProto.ActionList,
                Handler = HandlerTypeProto.HandlerGroup,
                Payload = Any.Pack(new GroupProto()
                    //Must put in a payload, do not leave null otherwise there will be an exception on the Java server
                    {
                        Id = 0,
                        Name = "DEFAULT"
                    })
            };

            var response = _handler.SendRequestAsync(request);
            GroupProtoList received = response.Result.Payload.Unpack<GroupProtoList>();

            List<Group> groups = new();
            foreach (GroupProto groupProto in received.Groups)
            {
                groups.Add(new Group.Builder()
                    .SetId(groupProto.Id)
                    .SetName(groupProto.Name)
                    .Build());
            }
            return groups.AsQueryable();
    }

    private RequestProto MakeRequestProto(ActionTypeProto ActionNeeded, GroupProto payload)
    {
        return new RequestProto()
        {
            Action = ActionNeeded,
            Handler = HandlerTypeProto.HandlerGroup,
            Payload = Any.Pack(payload)
        };
    }
}