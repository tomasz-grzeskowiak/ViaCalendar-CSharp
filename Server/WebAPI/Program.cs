using GrpcAPI;
using GrpcAPI.Services;
using PersistenceContracts;
using PersistenceHandlerGrpc.EventPersistence;
using PersistenceHandlerGrpc.UserPersistence;
using Services.Event;
using Services.User;

var builder = WebApplication.CreateBuilder(args);
//Add controllers to the container 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//add more services
builder.Services.AddScoped<EventServiceProto>();
builder.Services.AddScoped<UserServiceProto>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddKeyedScoped<ICalendarPersistenceHandler, EventHandlerGrpc>("event");
builder.Services.AddKeyedScoped<ICalendarPersistenceHandler, UserHandlerGrpc>("user");
builder.Services.AddSingleton<CalendarMainGrpcHandler>(sp =>
{
    var channel =
        Grpc.Net.Client.GrpcChannel.ForAddress("http://localhost:6032");
    return new CalendarMainGrpcHandler(channel);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();