using GrpcAPI;
using GrpcAPI.Services;
using PersistenceContracts;
using PersistenceHandlerGrpc.CalendarPersistence;
using PersistenceHandlerGrpc.EventPersistence;
using PersistenceHandlerGrpc.UserPersistence;
using PersistenceHandlerGrpc.GroupPersistence;
using Services.Calendar;
using Services.Event;
using Services.User;
using Services.Group;

var builder = WebApplication.CreateBuilder(args);
//Add controllers 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//add services
builder.Services.AddScoped<EventServiceProto>();
builder.Services.AddScoped<UserServiceProto>();
builder.Services.AddScoped<GroupServiceProto>();
builder.Services.AddScoped<CalendarServiceProto>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddKeyedScoped<ICalendarPersistenceHandler, EventHandlerGrpc>("event");
builder.Services.AddKeyedScoped<ICalendarPersistenceHandler, UserHandlerGrpc>("user");
builder.Services.AddKeyedScoped<ICalendarPersistenceHandler, GroupHandlerGrpc>("group");
builder.Services.AddKeyedScoped<ICalendarPersistenceHandler, CalendarHandlerGrpc>("calendar");

builder.Services.AddSingleton<CalendarMainGrpcHandler>(sp =>
{
    var channel =
        Grpc.Net.Client.GrpcChannel.ForAddress("http://localhost:6032");
    return new CalendarMainGrpcHandler(channel);
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();