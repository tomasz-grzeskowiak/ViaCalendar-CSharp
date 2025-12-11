using System.Runtime.InteropServices.JavaScript;
using RepositoryContracts;
using Entities;
using Google.Protobuf.WellKnownTypes;
using GrpcAPI.Protos;

namespace GrpcAPI.Services;

public class CalendarServiceProto : ICalendarRepository
{
    private readonly CalendarMainGrpcHandler _handler;

    public CalendarServiceProto(CalendarMainGrpcHandler handler)
    {
        _handler = handler;
    }
    public async Task<Calendar> CreateAsync(Calendar payload)
    {
        var proto = new CalendarProto()
        {
            UserId = payload.UserId,
        };
        if (payload.EventIds != null && payload.EventIds.Any())
        {
            proto.EventListId.AddRange(payload.EventIds);
        }
        
        var request = MakeRequestProto(ActionTypeProto.ActionCreate, proto);

        var response = await _handler.SendRequestAsync(request);

        var created = response.Payload.Unpack<CalendarProto>();

        return new Calendar.Builder()
            .SetUserId(created.UserId)
            .SetEventIds(created.EventListId.ToList()) 
            .Build();
    }

    public async Task UpdateAsync(Calendar payload)
    {
        var proto = new CalendarProto()
        {
            Id = payload.Id,
            UserId = payload.UserId,
        };
        
        if (payload.EventIds != null)
        {
            proto.EventListId.AddRange(payload.EventIds);
        }
        
        var update = MakeRequestProto(ActionTypeProto.ActionUpdate, proto);
        await _handler.SendRequestAsync(update);
    }

    public async Task DeleteAsync(int id)
    {
        var proto = new CalendarProto()
        {
            Id =id,
        };
        var delete = MakeRequestProto(ActionTypeProto.ActionDelete, proto);
        await _handler.SendRequestAsync(delete);
    }

    public async Task<Calendar> GetSingleAsync(int id)
    {
        var proto = new CalendarProto()
        {
            Id = id,
        };
        var fetched = MakeRequestProto(ActionTypeProto.ActionGet, proto);
        var response = await _handler.SendRequestAsync(fetched);
        var calendarProto = response.Payload.Unpack<CalendarProto>()
                      ?? throw new InvalidDataException("Calendar not found");
        return new Calendar.Builder()
            .SetId(calendarProto.Id)
            .SetUserId(calendarProto.UserId)
            .SetEventIds(calendarProto.EventListId.ToList())
            .Build();
    }

    public IQueryable<Calendar> GetManyAsync()
    {
            RequestProto request = new()
            {
                Action = ActionTypeProto.ActionList,
                Handler = HandlerTypeProto.HandlerCalendar,
                Payload = Any.Pack(new CalendarProto()
                    //Must put in a payload, do not leave null otherwise there will be an exception on the Java server
                    {
                        Id = 0,
                    })
            };

            var response = _handler.SendRequestAsync(request);
            CalendarProtoList received = response.Result.Payload.Unpack<CalendarProtoList>();

            List<Calendar> calendars = new();
            foreach (CalendarProto calendarProto in received.Calendars)
            {
                calendars.Add(new Calendar.Builder()
                    .SetId(calendarProto.Id)
                    .SetUserId(calendarProto.UserId)
                    .SetEventIds(calendarProto.EventListId.ToList())
                    .Build());
            }
            return calendars.AsQueryable();
    }

    private RequestProto MakeRequestProto(ActionTypeProto ActionNeeded, CalendarProto payload)
    {
        return new RequestProto()
        {
            Action = ActionNeeded,
            Handler = HandlerTypeProto.HandlerCalendar,
            Payload = Any.Pack(payload)
        };
    }
}