using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Pages.OnlineDating;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Layout.Main;

public partial class TopBarComponent : IDisposable
{
    [CascadingParameter]
    public SessionState? SessionState { get; set; }

    private RenderFragment? _additionalLogo;
    private bool _registeredAsPageSelectionListener = false;


    protected override Task OnParametersSetAsync()
    {
        if(!_registeredAsPageSelectionListener && SessionState != null)
        {
            SessionState.CurrentSelectedPageChanged += OnPageSelectionChanged;
            _registeredAsPageSelectionListener = true;
        }
        return base.OnParametersSetAsync();
    }

    private void OnPageSelectionChanged(object? sender, SessionSelectedPage? newPageSelection)
    {
        if(newPageSelection == null)
        {
            _additionalLogo = null;
        }
        else if(newPageSelection == SessionSelectedPage.OnlineDating)
        {
            _additionalLogo = (builder) =>
            {
                builder.OpenComponent<OnlineDatingLogoComponent>(0);
                builder.AddAttribute(1, nameof(OnlineDatingLogoComponent.Height), 100);
                builder.CloseComponent();
            };
        }
        SessionState?.OnUpdate();
    }

    public void Dispose()
    {
        if (_registeredAsPageSelectionListener && SessionState != null)
        {
            SessionState.CurrentSelectedPageChanged -= OnPageSelectionChanged;
        }
    }

}
