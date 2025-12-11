using System.Runtime.InteropServices.JavaScript;
using RepositoryContracts;
using Entities;
using Google.Protobuf.WellKnownTypes;
using GrpcAPI.Protos;

namespace GrpcAPI.Services;

public class EventServiceProto : IEventRepository
{
    private readonly CalendarMainGrpcHandler _handler;

    public EventServiceProto(CalendarMainGrpcHandler handler)
    {
        _handler = handler;
    }
    public async Task<Event> CreateAsync(Event payload)
    {
        var proto = new EventProto()
        {
            Name = payload.Name,
            Recursive = payload.Recursive,
            Tag = payload.Tag,
            CreatorId = payload.CreatorId,
            Start = Timestamp.FromDateTime(payload.Start.ToUniversalTime()),
            End = Timestamp.FromDateTime(payload.End.ToUniversalTime())
        };
    
        // Convert string to TypeOfRecursiveProto enum
        if (!string.IsNullOrEmpty(payload.TypeOfRecursive))
        {
            proto.TypeOfRecursive = payload.TypeOfRecursive.ToUpper() switch
            {
                "DAY" or "DAILY" => TypeOfRecursiveProto.Day,
                "MONTH" or "MONTHLY" => TypeOfRecursiveProto.Month,
                "YEAR" or "YEARLY" => TypeOfRecursiveProto.Year,
                _ => TypeOfRecursiveProto.None
            };
        }
    
        var request = MakeRequestProto(ActionTypeProto.ActionCreate, proto);

        var response = await _handler.SendRequestAsync(request);

        var created = response.Payload.Unpack<EventProto>();
    
        return new Event.Builder()
            .SetName(created.Name)
            .SetRecursive(created.Recursive)
            .SetTag(created.Tag)
            .SetCreatorId(created.CreatorId)
            .SetStart(created.Start.ToDateTime())
            .SetEnd(created.End.ToDateTime())
            .SetTypeOfRecursive(created.TypeOfRecursive switch
            {
                TypeOfRecursiveProto.Day => "Day",
                TypeOfRecursiveProto.Month => "Month",
                TypeOfRecursiveProto.Year => "Year",
                _ => ""
            })
            .Build();
    }

    public async Task UpdateAsync(Event payload)
    {
        var proto = new EventProto()
        {
            Id = payload.Id,
            Name = payload.Name,
            Recursive = payload.Recursive,
            Tag = payload.Tag,
            CreatorId = payload.CreatorId,
            Start = Timestamp.FromDateTime(payload.Start.ToUniversalTime()),
            End = Timestamp.FromDateTime(payload.End.ToUniversalTime())
        };
    
        // Convert string to TypeOfRecursiveProto enum
        if (!string.IsNullOrEmpty(payload.TypeOfRecursive))
        {
            proto.TypeOfRecursive = payload.TypeOfRecursive.ToUpper() switch
            {
                "DAY" or "DAILY" => TypeOfRecursiveProto.Day,
                "MONTH" or "MONTHLY" => TypeOfRecursiveProto.Month,
                "YEAR" or "YEARLY" => TypeOfRecursiveProto.Year,
                _ => TypeOfRecursiveProto.None
            };
        }
        else
        {
            proto.TypeOfRecursive = TypeOfRecursiveProto.None;
        }
    
        var update = MakeRequestProto(ActionTypeProto.ActionUpdate, proto);
        await _handler.SendRequestAsync(update);
    }

    public async Task DeleteAsync(int id)
    {
        var proto = new EventProto()
        {
            Id =id,
        };
        var delete = MakeRequestProto(ActionTypeProto.ActionDelete, proto);
        await _handler.SendRequestAsync(delete);
    }

    public async Task<Event> GetSingleAsync(int id)
    {
        var proto = new EventProto() { Id = id };
        var fetched = MakeRequestProto(ActionTypeProto.ActionGet, proto);
        var response = await _handler.SendRequestAsync(fetched);
        var eventProto = response.Payload.Unpack<EventProto>()
                         ?? throw new InvalidDataException("Event not found");

        return new Event.Builder()
            .SetId(eventProto.Id)
            .SetName(eventProto.Name)
            .SetRecursive(eventProto.Recursive)
            .SetTag(eventProto.Tag)
            .SetCreatorId(eventProto.CreatorId)
            .SetStart(eventProto.Start.ToDateTime())
            .SetEnd(eventProto.End.ToDateTime())
            .SetTypeOfRecursive(eventProto.TypeOfRecursive switch
            {
                TypeOfRecursiveProto.Day => "Day",
                TypeOfRecursiveProto.Month => "Month",
                TypeOfRecursiveProto.Year => "Year",
                _ => ""
            })
            .Build();
    }

    public IQueryable<Event> GetManyAsync()
    {
        RequestProto request = new()
        {
            Action = ActionTypeProto.ActionList,
            Handler = HandlerTypeProto.HandlerEvent,
            Payload = Any.Pack(new EventProto()
            {
                Id = 0,
                Name = "DEFAULT"
            })
        };

        var response = _handler.SendRequestAsync(request);
        EventProtoList received = response.Result.Payload.Unpack<EventProtoList>();

        List<Event> events = new();
        foreach (EventProto eventProto in received.Events)
        {
            events.Add(new Event.Builder()
                .SetId(eventProto.Id)
                .SetName(eventProto.Name)
                .SetRecursive(eventProto.Recursive)
                .SetTag(eventProto.Tag)
                .SetCreatorId(eventProto.CreatorId)
                .SetStart(eventProto.Start.ToDateTime())
                .SetEnd(eventProto.End.ToDateTime())
                .SetTypeOfRecursive(eventProto.TypeOfRecursive switch
                {
                    TypeOfRecursiveProto.Day => "Day",
                    TypeOfRecursiveProto.Month => "Month",
                    TypeOfRecursiveProto.Year => "Year",
                    _ => ""
                })
                .Build());
        }
        return events.AsQueryable();
    }

    private RequestProto MakeRequestProto(ActionTypeProto ActionNeeded, EventProto payload)
    {
        return new RequestProto()
        {
            Action = ActionNeeded,
            Handler = HandlerTypeProto.HandlerEvent,
            Payload = Any.Pack(payload)
        };
    }
}