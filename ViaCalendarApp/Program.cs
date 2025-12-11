using ViaCalendarApp.Components;
using ViaCalendarApp.Services;
using ViaCalendarApp.Helper;
using Syncfusion.Blazor;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjGyl/Vkd+XU9FcVRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3tSdERiWHpceXdXT2ZYWE91Xg==");

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddRazorPages();             
builder.Services.AddServerSideBlazor();       
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<StorageHelper>();
builder.Services.AddSyncfusionBlazor();

// Register EventServiceClient
var eventHttpClientBuilder = builder.Services.AddHttpClient<EventServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7259");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Register UserServiceClient
var userHttpClientBuilder = builder.Services.AddHttpClient<UserServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7259"); // Same base address or adjust as needed
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Register GroupServiceClient
var groupHttpClientBuilder = builder.Services.AddHttpClient<GroupServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7259"); // Same base address or adjust as needed
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Register CalendarServiceClient
var calendarHttpClientBuilder = builder.Services.AddHttpClient<CalendarServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7259"); // Same base address or adjust as needed
    client.Timeout = TimeSpan.FromSeconds(10);
});

if (builder.Environment.IsDevelopment())
{
    eventHttpClientBuilder.ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
    
    userHttpClientBuilder.ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
    
    groupHttpClientBuilder.ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
    
    calendarHttpClientBuilder.ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });
}

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Add this if you have static files
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();