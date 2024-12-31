using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
using SuneDoes.UI.Pages.Home;
using SuneDoes.UI.Pages.LucidDreaming;
using SuneDoes.UI.Pages.Medicine;
using SuneDoes.UI.Pages.Meditation;
using SuneDoes.UI.Pages.OnlineDating;
using System.Reflection;

namespace SuneDoes.UI.Session;

public record SessionState(Action OnUpdate, IServiceScopeFactory ScopeFactory)
{
    private SessionSelectedPage? _selectedPage;
    public SessionSelectedPage? SelectedPage { 
        get => _selectedPage; 
        set {
            _selectedPage = value;
        }
    }

    private string? _currentShowImagesTitle;
    public string? CurrentShowImagesTitle => _currentShowImagesTitle;

    private IReadOnlyCollection<ImageShowComponent.ShowImage>? _currentShowImages;
    public IReadOnlyCollection<ImageShowComponent.ShowImage>? CurrentShowImages => 
        _currentShowImages;

    private bool _useDarkModeForImages = false;
    public bool UseDarkModeForImages => _useDarkModeForImages;
    public void ShowImages(string curremtShowImagesTitle, bool useDarkMode = false, params ImageShowComponent.ShowImage[] images)
    {
        _currentShowImagesTitle = curremtShowImagesTitle;
        _currentShowImages = images;
        _useDarkModeForImages = useDarkMode;
        OnUpdate();
    }
    public void StopShowImages()
    {
        _currentShowImagesTitle = null;
        _currentShowImages = null;
        OnUpdate();
    }

    private bool _sideBarExpanded = false;
    public bool SideBarExpanded { get => _sideBarExpanded; set { _sideBarExpanded = value; OnUpdate(); } }


    private static readonly Dictionary<Type, SessionSelectedPage> TypeToPageMap = new Dictionary<Type, SessionSelectedPage>
    {
        {typeof(HomePage), SessionSelectedPage.Home},
        {typeof(OnlineDatingPage), SessionSelectedPage.OnlineDating},
        {typeof(MeditationPage), SessionSelectedPage.Meditation},
        {typeof(LucidDreamingPage), SessionSelectedPage.LucidDreaming},
        {typeof(MedicinePage), SessionSelectedPage.Medicine}
    };

    public SessionSelectedPage? CurrentPage(NavigationManager navManager)
    {
        var absoluteUrl = navManager.Uri;

        var currentUrl = navManager.ToBaseRelativePath(absoluteUrl);
        if (currentUrl == null)
            return null;
        if (!currentUrl.StartsWith("/"))
            currentUrl = "/" + currentUrl;

        var searchValues = TypeToPageMap
            .Select(pair => (PageType: pair.Key, SessionPageType: pair.Value, TemplateUrl: pair.Key.GetCustomAttribute<RouteAttribute>()?.Template?.ToLower()?.Trim()))
            .Where(_ => _.TemplateUrl != null)
            .ToList();

        var matchingPage = searchValues
            .Where(_ =>
                  (currentUrl.Length < 2 && _.TemplateUrl!.Length < 2) ||
                  (_.TemplateUrl!.Length > 2 && currentUrl.Contains(_.TemplateUrl!))
            ).Select(_ => _.SessionPageType)
            .FirstOrDefault();
        return matchingPage;

    }

}
