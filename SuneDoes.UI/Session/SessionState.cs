using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
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
            if (_selectedPage != value)
            {
                _selectedPage = value;
                CurrentSelectedPageChanged?.Invoke(this,_selectedPage);

            }
        } }

    private string? _currentShowImagesTitle;
    public string? CurrentShowImagesTitle => _currentShowImagesTitle;

    private IReadOnlyCollection<ImageShowComponent.ShowImage>? _currentShowImages;
    public IReadOnlyCollection<ImageShowComponent.ShowImage>? CurrentShowImages => 
        _currentShowImages;
    public void ShowImages(string curremtShowImagesTitle, params ImageShowComponent.ShowImage[] images)
    {
        _currentShowImagesTitle = curremtShowImagesTitle;
        _currentShowImages = images;
        OnUpdate();
    }
    public void StopShowImages()
    {
        _currentShowImagesTitle = null;
        _currentShowImages = null;
        OnUpdate();
    }

    public event EventHandler<SessionSelectedPage?> CurrentSelectedPageChanged;

    private bool _sideBarExpanded = false;
    public bool SideBarExpanded { get => _sideBarExpanded; set { _sideBarExpanded = value; OnUpdate(); } }




}
