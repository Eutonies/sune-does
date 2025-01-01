using SuneDoes.UI.Components;
using SuneDoes.UI.Components.Email;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Pages.Medicine;
using SuneDoes.UI.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
   .AddJsonFile("appsettings.local.json", optional: true)
   .AddEnvironmentVariables();
builder.Services.Configure<SuneDoesConfiguration>(builder.Configuration);
var appConfig = new SuneDoesConfiguration();
builder.Configuration.Bind(appConfig);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
Console.WriteLine($"Using base path: {appConfig.HostingBasePath}");
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ISuneDoesEmailSender, SuneDoesEmailSender>();
builder.AddSuneDoesDbContext();
builder.Services.AddSingleton<IVerifiableEmailHandler, VerifiableEmailHandler>();
builder.Services.AddSingleton<IMedicineNotificationHandler, MedicineNotificationHandler>();

var app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

if (!string.IsNullOrEmpty(appConfig.HostingBasePath))
    app.MapBlazorHub("/" + appConfig.HostingBasePath)
    .WithOrder(-1);
/*else app.MapBlazorHub()
    .WithOrder(-1);*/

app.UseRouting();
app.UseAntiforgery();

app.Run();
