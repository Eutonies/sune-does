using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SuneDoes.UI.Pages.Meditation;
using SuneDoes.UI.Pages.OnlineDating;
using SuneDoes.UI.Session;
using System.Reflection;

namespace SuneDoes.UI.Layout.Main;

public partial class SideNavigationBar
{
    [CascadingParameter]
    public SessionState SessionState { get; set; }


    private static string? TargetUrlFor(SessionSelectedPage page)
    {
        var typ = page switch
        {
            SessionSelectedPage.Meditation => typeof(MeditationPage),
            _ => typeof(OnlineDatingPage)
        };
        var routeAttr = typ.GetCustomAttribute<RouteAttribute>();
        var returnee = routeAttr?.Template;
        return returnee;
    }


}
