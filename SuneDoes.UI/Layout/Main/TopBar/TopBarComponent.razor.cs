using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Pages.Home;
using SuneDoes.UI.Pages.Meditation;
using SuneDoes.UI.Pages.OnlineDating;
using SuneDoes.UI.Session;
using System.Reflection;

namespace SuneDoes.UI.Layout.Main.TopBar;

public partial class TopBarComponent 
{

    [Inject]
    public NavigationManager NavManager { get; set; }

    [CascadingParameter]
    public SessionState? SessionState { get; set; }

    private RenderFragment? _additionalLogo;
    private SessionSelectedPage? _currentLogoPage;



    protected override void OnAfterRender(bool firstRender)
    {
        UpdateLogo();
    }

    protected override void OnInitialized()
    {
        UpdateLogo();
    }

    private void UpdateLogo()
    {
        if (SessionState != null)
        {
            var currentPage = SessionState.CurrentPage(NavManager);
            if (currentPage != _currentLogoPage)
                OnPageSelectionChanged(currentPage);
        }
    }


    private void OnPageSelectionChanged(SessionSelectedPage? newPageSelection)
    {
        if (newPageSelection == null || newPageSelection == SessionSelectedPage.Home)
        {
            _additionalLogo = null;
        }
        else if (newPageSelection == SessionSelectedPage.OnlineDating)
        {
            _additionalLogo = (builder) =>
            {
                builder.OpenComponent<OnlineDatingLogoComponent>(0);
                builder.AddAttribute(1, nameof(OnlineDatingLogoComponent.Height), 100);
                builder.CloseComponent();
            };
        }
        else if (newPageSelection == SessionSelectedPage.Meditation)
        {
            _additionalLogo = (builder) =>
            {
                builder.OpenComponent<MeditationLogoComponent>(0);
                builder.AddAttribute(1, nameof(MeditationLogoComponent.Height), 100);
                builder.CloseComponent();
            };
        }
        _currentLogoPage = newPageSelection;
        InvokeAsync(StateHasChanged);
    }

    private static string HomeLink => typeof(HomePage)
        .GetCustomAttribute<RouteAttribute>()!
        .Template;

}
