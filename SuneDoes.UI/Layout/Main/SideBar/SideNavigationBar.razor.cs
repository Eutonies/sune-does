using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Pages.Meditation;
using SuneDoes.UI.Pages.OnlineDating;
using SuneDoes.UI.Session;
using System.Reflection;

namespace SuneDoes.UI.Layout.Main.SideBar;

public partial class SideNavigationBar
{
    [CascadingParameter]
    public SessionState SessionState { get; set; }

    [Inject]
    public IOptions<SuneDoesConfiguration> AppConfig { get; set; }


    private string? TargetUrlFor(SessionSelectedPage page)
    {
        var typ = page switch
        {
            SessionSelectedPage.Meditation => typeof(MeditationPage),
            _ => typeof(OnlineDatingPage)
        };
        var routeAttr = typ.GetCustomAttribute<RouteAttribute>();
        var url = routeAttr?.Template; 
        var basePath = AppConfig?.Value?.HostingBasePath;
        var returnee = string.IsNullOrEmpty(basePath) ? url : ("/" + basePath + url);
        return returnee;
    }


}
