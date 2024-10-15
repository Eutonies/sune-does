using SuneDoes.UI.Components;
using SuneDoes.UI.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
   .AddJsonFile("appsettings.local.json", optional: true)
   .AddEnvironmentVariables();
builder.Services.Configure<SuneDoesConfiguration>(builder.Configuration);
var appConfig = new SuneDoesConfiguration();
builder.Configuration.Bind(appConfig);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

if (!string.IsNullOrEmpty(appConfig.HostingBasePath))
    app.MapBlazorHub(appConfig.HostingBasePath);
else app.MapBlazorHub();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.UseRouting();
app.UseAntiforgery();

app.Run();
