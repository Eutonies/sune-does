using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Extensions;
using SuneDoes.UI.Pages.LucidDreaming;
using SuneDoes.UI.Pages.OnlineDating;
using System.Reflection;

namespace SuneDoes.UI.Pages.Home;

public partial class HomeLucidDreamingComponent
{
    [Inject]
    public NavigationManager NavManager { get; set; }


    [Inject]
    public IOptions<SuneDoesConfiguration> AppConfig { get; set; }

    private void OnImageClick(MouseEventArgs ev) => 
        NavManager.NavigateTo<LucidDreamingPage>(AppConfig);

}
