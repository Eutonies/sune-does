using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Extensions;
using SuneDoes.UI.Pages.LucidDreaming;
using SuneDoes.UI.Pages.OnlineDating;
using SuneDoes.UI.Pages.Shrapnel;
using SuneDoes.UI.Pages.Space;
using System.Reflection;

namespace SuneDoes.UI.Pages.Home;

public partial class HomeSpaceComponent
{
    [Inject]
    public NavigationManager NavManager { get; set; }


    [Inject]
    public IOptions<SuneDoesConfiguration> AppConfig { get; set; }

    private void OnImageClick(MouseEventArgs ev) => 
        NavManager.NavigateTo<SpacePage>(AppConfig);

}
