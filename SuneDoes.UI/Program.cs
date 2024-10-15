using SuneDoes.UI.Components;
using SuneDoes.UI.Configuration;

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
