using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Pages.Home;
using SuneDoes.UI.Pages.LucidDreaming;
using SuneDoes.UI.Pages.Medicine;
using SuneDoes.UI.Pages.Meditation;
using SuneDoes.UI.Pages.OnlineDating;
using SuneDoes.UI.Pages.Shrapnel;
using SuneDoes.UI.Pages.Space;
using SuneDoes.UI.Session;
using System.Reflection;

namespace SuneDoes.UI.Layout.Main.TopBar;

public partial class TopBarComponent 
{

    [Inject]
    public NavigationManager NavManager { get; set; }

    [CascadingParameter]
    public SessionState? SessionState { get; set; }

    [Inject]
    public IOptions<SuneDoesConfiguration> AppConfig { get; set; }

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
        else if(newPageSelection == SessionSelectedPage.LucidDreaming)
        {
            _additionalLogo = (builder) =>
            {
                builder.OpenComponent<LucidDreamingLogoComponent>(0);
                builder.AddAttribute(1, nameof(LucidDreamingLogoComponent.Height), 100);
                builder.CloseComponent();
            };
        }
        else if (newPageSelection == SessionSelectedPage.Medicine)
        {
            _additionalLogo = (builder) =>
            {
                builder.OpenComponent<MedicineLogoComponent>(0);
                builder.AddAttribute(1, nameof(MedicineLogoComponent.Height), 100);
                builder.CloseComponent();
            };
        }
        else if (newPageSelection == SessionSelectedPage.Shrapnel)
        {
            _additionalLogo = (builder) =>
            {
                builder.OpenComponent<ShrapnelLogoComponent>(0);
                builder.AddAttribute(1, nameof(ShrapnelLogoComponent.Height), 100);
                builder.CloseComponent();
            };
        }
        else if (newPageSelection == SessionSelectedPage.SpaceAndTime)
        {
            _additionalLogo = (builder) =>
            {
                builder.OpenComponent<SpaceLogoComponent>(0);
                builder.AddAttribute(1, nameof(SpaceLogoComponent.Height), 100);
                builder.CloseComponent();
            };
        }

        _currentLogoPage = newPageSelection;
        InvokeAsync(StateHasChanged);
    }

    private string? BasePath => AppConfig.Value.HostingBasePath;

    private string LogoPath => _currentLogoPage switch 
    { 
        SessionSelectedPage.Blocks => "images/main/sune-does-cold.webp",
        _ => "images/main/sune-does.webp"
    };

    private string BackgroundPath => _currentLogoPage switch
    {
        SessionSelectedPage.Blocks => "images/main/cold-background.webp",
        _ => "images/main/gold.webp"
    };

    private string HomeLink => BasePath switch {
        null => "",
        string bp => "/" + bp
    } + typeof(HomePage)
        .GetCustomAttribute<RouteAttribute>()!
        .Template;

}
