using Microsoft.AspNetCore.Components;
using SuneDoes.Extensions;
using SuneDoes.UI.Session;


namespace SuneDoes.UI.Pages.Home;

public partial class HomePage 
{
    [Inject]
    public IHttpContextAccessor ContextAccessor { get; set; }

    [Inject]
    public ILogger<HomePage> Logger { get; set; }

    protected override Task OnParametersSetAsync()
    {
        if (SessionState != null)
        {
            SessionState.SelectedPage = SessionSelectedPage.Home;
            if(ContextAccessor?.HttpContext?.Request != null) 
            {
                var req = ContextAccessor.HttpContext.Request!;
                Logger.LogInformation($"Received request for path: {req.Path}");
                Logger.LogInformation($"  - base path: {req.PathBase}");
                Logger.LogInformation($"  Headers:");
                foreach(var header in req.Headers.OrderBy(_ => _.Key))
                   Logger.LogInformation($"  - {header.Key}: {header.Value.MakeString(",")}");

                
            }
        }
        return base.OnParametersSetAsync();
    }

}
