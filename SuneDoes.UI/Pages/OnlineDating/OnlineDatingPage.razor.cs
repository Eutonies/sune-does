
using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.OnlineDating;

public partial class OnlineDatingPage
{
    private static ImageShowComponent.ShowImage Image(string app, string imageName, string name) =>
        new($"images/online-dating/{app}/{imageName}", name);
    private static ImageShowComponent.ShowImage TinderImage(string imageName, string name) => Image("tinder", imageName, name);

    protected override Task OnParametersSetAsync()
    {
        if(SessionState!= null && SessionState.SelectedPage != Session.SessionSelectedPage.OnlineDating)
        {
            SessionState.SelectedPage = Session.SessionSelectedPage.OnlineDating;
        }
        return base.OnParametersSetAsync();
    }

    protected override Task OnInitializedAsync()
    {
        if(SessionState.CurrentShowImages == null)
        {
            SessionState.ShowImages("Online Dating Images",
                TinderImage("tinder-frontpage.png", "Adventure"),
                TinderImage("sune-w-quote-1.png", "Jesper endorsement 1/3"),
                TinderImage("sune-w-quote-2.png", "Jesper endorsement 2/3"),
                TinderImage("sune-w-quote-3.png", "Jesper endorsement 3/3")
            );
        }
        return Task.CompletedTask;
    }

}
