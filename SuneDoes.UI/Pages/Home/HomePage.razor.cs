using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.Home;

public partial class HomePage 
{

    protected override Task OnParametersSetAsync()
    {
        if (SessionState != null && SessionState.SelectedPage != null)
        {
            SessionState.SelectedPage = null;
        }
        return base.OnParametersSetAsync();
    }

}
