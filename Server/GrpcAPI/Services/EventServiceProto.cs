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
        };
    
        // Convert DateTime? to Timestamp for Duration
        if (payload.Duration.HasValue)
        {
            proto.Duration = Timestamp.FromDateTime(payload.Duration.Value.ToUniversalTime());
        }
    
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

        // Convert back for the returned Event
        DateTime? duration = null;
        if (created.Duration != null && created.Duration.Seconds > 0)
        {
            duration = DateTime.UnixEpoch.AddSeconds(created.Duration.Seconds);
        }
    
        string typeOfRecursive = created.TypeOfRecursive switch
        {
            TypeOfRecursiveProto.Day => "Day",
            TypeOfRecursiveProto.Month => "Month",
            TypeOfRecursiveProto.Year => "Year",
            _ => ""
        };

        return new Event.Builder()
            .SetName(created.Name)
            .SetRecursive(created.Recursive)
            .SetTag(created.Tag)
            .SetCreatorId(created.CreatorId)
            .SetDuration(duration)  // Pass DateTime? 
            .SetTypeOfRecursive(typeOfRecursive)  // Pass string
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
        };
    
        // Converts DateTime? to Timestamp for Duration
        if (payload.Duration.HasValue)
        {
            proto.Duration = Timestamp.FromDateTime(payload.Duration.Value.ToUniversalTime());
        }
        else
        {
            // Set empty timestamp if null
            proto.Duration = new Timestamp();
        }
    
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
        var proto = new EventProto()
        {
            Id = id,
        };
        var fetched = MakeRequestProto(ActionTypeProto.ActionGet, proto);
        var response = await _handler.SendRequestAsync(fetched);
        var eventProto = response.Payload.Unpack<EventProto>() 
                         ?? throw new InvalidDataException("Event not found");
    
        // Convert Timestamp to DateTime? for Duration
        DateTime? duration = null;
        if (eventProto.Duration != null && eventProto.Duration.Seconds > 0)
        {
            duration = DateTime.UnixEpoch.AddSeconds(eventProto.Duration.Seconds);
        }
    
        // Convert TypeOfRecursiveProto enum to string
        string typeOfRecursive = eventProto.TypeOfRecursive switch
        {
            TypeOfRecursiveProto.Day => "Day",
            TypeOfRecursiveProto.Month => "Month",
            TypeOfRecursiveProto.Year => "Year",
            _ => ""
        };

        return new Event.Builder()
            .SetId(eventProto.Id)
            .SetName(eventProto.Name)
            .SetRecursive(eventProto.Recursive)
            .SetTag(eventProto.Tag)
            .SetCreatorId(eventProto.CreatorId)
            .SetDuration(duration)  // Pass DateTime?
            .SetTypeOfRecursive(typeOfRecursive)  // Pass string
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
            // Convert Timestamp to DateTime? for Duration
            DateTime? duration = null;
            if (eventProto.Duration != null && eventProto.Duration.Seconds > 0)
            {
                duration = DateTime.UnixEpoch.AddSeconds(eventProto.Duration.Seconds);
            }
        
            // Convert TypeOfRecursiveProto enum to string
            string typeOfRecursive = eventProto.TypeOfRecursive switch
            {
                TypeOfRecursiveProto.Day => "Day",
                TypeOfRecursiveProto.Month => "Month",
                TypeOfRecursiveProto.Year => "Year",
                _ => ""
            };

            events.Add(new Event.Builder()
                .SetId(eventProto.Id)
                .SetName(eventProto.Name)
                .SetRecursive(eventProto.Recursive)
                .SetTag(eventProto.Tag)
                .SetCreatorId(eventProto.CreatorId)
                .SetDuration(duration)  
                .SetTypeOfRecursive(typeOfRecursive)  
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