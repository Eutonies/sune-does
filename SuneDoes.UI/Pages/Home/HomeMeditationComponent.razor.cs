using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SuneDoes.UI.Pages.Meditation;
using SuneDoes.UI.Pages.OnlineDating;
using System.Reflection;

namespace SuneDoes.UI.Pages.Home;

public partial class HomeMeditationComponent
{
    [Inject]
    public NavigationManager NavManager { get; set; }

    private void OnImageClick(MouseEventArgs ev)
    {
        var url = typeof(MeditationPage)
            .GetCustomAttribute<RouteAttribute>()!
            .Template;
        NavManager.NavigateTo(url);
    }

}
