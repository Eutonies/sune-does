
namespace SuneDoes.UI.Pages.OnlineDating;

public partial class OnlineDatingPage
{
    protected override Task OnParametersSetAsync()
    {
        if(SessionState!= null && SessionState.SelectedPage != Session.SessionSelectedPage.OnlineDating)
        {
            SessionState.SelectedPage = Session.SessionSelectedPage.OnlineDating;
        }
        return base.OnParametersSetAsync();
    }

}
