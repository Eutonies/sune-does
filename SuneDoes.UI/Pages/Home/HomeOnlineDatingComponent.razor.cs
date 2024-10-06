using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SuneDoes.UI.Pages.OnlineDating;
using System.Reflection;

namespace SuneDoes.UI.Pages.Home;

public partial class HomeOnlineDatingComponent
{
    [Inject]
    public NavigationManager NavManager { get; set; }

    private void OnImageClick(MouseEventArgs ev)
    {
        var url = typeof(OnlineDatingPage)
            .GetCustomAttribute<RouteAttribute>()!
            .Template;
        NavManager.NavigateTo(url);
    }

}
