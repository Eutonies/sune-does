using SuneDoes.UI.Session;

namespace SuneDoes.UI.Layout.Main;

public partial class MainLayout
{
    private readonly SessionState _sessionState;
    
    public MainLayout()
    {
        _sessionState = new SessionState(() => InvokeAsync(StateHasChanged));
    }


}
