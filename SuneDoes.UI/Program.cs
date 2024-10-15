using SuneDoes.UI.Components;
using SuneDoes.UI.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Configuration
   .AddJsonFile("appsettings.local.json", optional: true)
   .AddEnvironmentVariables();
builder.Services.Configure<SuneDoesConfiguration>(builder.Configuration);
var appConfig = new SuneDoesConfiguration();
builder.Configuration.Bind(appConfig);


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddHttpContextAccessor();    

var app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery();

if (!string.IsNullOrEmpty(appConfig.HostingBasePath))
    app.MapBlazorHub(appConfig.HostingBasePath)
    .WithOrder(-1);
else app.MapBlazorHub()
    .WithOrder(-1);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.UseRouting();
app.UseAntiforgery();

app.Run();
