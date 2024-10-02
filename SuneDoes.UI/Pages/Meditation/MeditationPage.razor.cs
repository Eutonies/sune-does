
using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.Meditation;

public partial class MeditationPage
{
    protected override Task OnParametersSetAsync()
    {
        if(SessionState!= null && SessionState.SelectedPage != SessionSelectedPage.Meditation)
        {
            SessionState.SelectedPage = SessionSelectedPage.Meditation;
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
