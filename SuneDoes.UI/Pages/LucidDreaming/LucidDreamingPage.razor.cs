
using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.LucidDreaming;

public partial class LucidDreamingPage
{
    protected override Task OnParametersSetAsync()
    {
        if(SessionState!= null)
        {
            SessionState.SelectedPage = Session.SessionSelectedPage.OnlineDating;
        }
        return base.OnParametersSetAsync();
    }

    protected override Task OnInitializedAsync()
    {
        if(SessionState.CurrentShowImages == null)
        {
            
        }
        return Task.CompletedTask;
    }


}
