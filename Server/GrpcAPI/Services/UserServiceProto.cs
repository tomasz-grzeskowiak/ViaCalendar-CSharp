using System.Runtime.InteropServices.JavaScript;
using RepositoryContracts;
using Entities;
using Google.Protobuf.WellKnownTypes;
using GrpcAPI.Protos;

namespace GrpcAPI.Services;

public class UserServiceProto : IUserRepository
{
    private readonly CalendarMainGrpcHandler _handler;

    public UserServiceProto(CalendarMainGrpcHandler handler)
    {
        _handler = handler;
    }
    public async Task<User> CreateAsync(User payload)
    {
        var proto = new UserProto()
        {
            Username = payload.UserName,
            Password = payload.Password,
            Email = payload.Email,
            FirstName = payload.FirstName,
            LastName = payload.LastName,
            GroupId = payload.GroupId
        };
        var request = MakeRequestProto(ActionTypeProto.ActionCreate, proto);

        var response = await _handler.SendRequestAsync(request);

        var created = response.Payload.Unpack<UserProto>();

        return new User.Builder()
            .SetUsername(created.Username)
            .SetEmail(created.Email)
            .SetFirstName(created.FirstName)
            .SetLastName(created.LastName)
            .SetPassword(created.Password)
            .SetGroupId(created.GroupId)
            .Build();
    }

    public async Task UpdateAsync(User payload)
    {
        var proto = new UserProto()
        {
            Id = payload.Id,
            Username = payload.UserName,
            Password = payload.Password,
            Email = payload.Email,
            FirstName = payload.FirstName,
            LastName = payload.LastName,
            GroupId = payload.GroupId
        };
        var update = MakeRequestProto(ActionTypeProto.ActionUpdate, proto);
        await _handler.SendRequestAsync(update);
    }

    public async Task DeleteAsync(int id)
    {
        var proto = new UserProto()
        {
            Id =id,
        };
        var delete = MakeRequestProto(ActionTypeProto.ActionDelete, proto);
        await _handler.SendRequestAsync(delete);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var proto = new UserProto()
        {
            Id = id,
        };
        var fetched = MakeRequestProto(ActionTypeProto.ActionGet, proto);
        var response = await _handler.SendRequestAsync(fetched);
        var userProto = response.Payload.Unpack<UserProto>()
                      ?? throw new InvalidDataException("User not found");
        return new User.Builder()
            .SetId(userProto.Id)
            .SetUsername(userProto.Username)
            .SetEmail(userProto.Email)
            .SetFirstName(userProto.FirstName)
            .SetLastName(userProto.LastName)
            .SetPassword(userProto.Password)
            .SetGroupId(userProto.GroupId)
            .Build();
    }

    public IQueryable<User> GetManyAsync()
    {
            RequestProto request = new()
            {
                Action = ActionTypeProto.ActionList,
                Handler = HandlerTypeProto.HandlerUser,
                Payload = Any.Pack(new UserProto()
                    //Must put in a payload, do not leave null otherwise there will be an exception on the Java server
                    {
                        Id = 0,
                        Username = "DEFAULT"
                    })
            };

            var response = _handler.SendRequestAsync(request);
            UserProtoList received = response.Result.Payload.Unpack<UserProtoList>();

            List<User> users = new();
            foreach (UserProto userProto in received.Users)
            {
                users.Add(new User.Builder()
                    .SetId(userProto.Id)
                    .SetUsername(userProto.Username)
                    .SetEmail(userProto.Email)
                    .SetFirstName(userProto.FirstName)
                    .SetLastName(userProto.LastName)
                    .SetPassword(userProto.Password)
                    .SetGroupId(userProto.GroupId)
                    .Build());
            }
            return users.AsQueryable();
    }

    private RequestProto MakeRequestProto(ActionTypeProto ActionNeeded, UserProto payload)
    {
        return new RequestProto()
        {
            Action = ActionNeeded,
            Handler = HandlerTypeProto.HandlerUser,
            Payload = Any.Pack(payload)
        };
    }
}