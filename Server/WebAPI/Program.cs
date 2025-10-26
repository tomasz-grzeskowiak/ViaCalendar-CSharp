using GrpcAPI.Services;
using PersistenceContracts;
using PersistenceHandlerGrpc.EventPersistence;
using Services.Event;

var builder = WebApplication.CreateBuilder(args);
//Add controllers to the container 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//add more services
builder.Services.AddScoped<EventServiceProto>();
builder.Services.AddScoped<ICalendarPersistenceHandler,EventHandlerGrpc>();
builder.Services.AddScoped<IEventService, EventService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();