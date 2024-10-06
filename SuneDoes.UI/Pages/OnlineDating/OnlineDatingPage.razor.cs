
using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.OnlineDating;

public partial class OnlineDatingPage
{
    private static ImageShowComponent.ShowImage Image(string app, string imageName, string name) =>
        new($"images/online-dating/{app}/{imageName}", name);
    private static ImageShowComponent.ShowImage TinderImage(string imageName, string name) => Image("tinder", imageName, name);

    private static ImageShowComponent.ShowImage HingeImage(string imageName, string name) => Image("hinge", imageName, name);

    private static ImageShowComponent.ShowImage HappnImage(string imageName, string name) => Image("happn", imageName, name);

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


    private static IReadOnlyCollection<ImageShowComponent.ShowImage> TinderImages = [ 
                TinderImage("tinder-frontpage.png", "Adventure"),
                TinderImage("sune-w-quote-1.png", "Jesper endorsement 1/3"),
                TinderImage("sune-w-quote-2.png", "Jesper endorsement 2/3"),
                TinderImage("sune-w-quote-3.png", "Jesper endorsement 3/3"),
                TinderImage("partner-in-crime.jpg", "Partner in crime"),
                TinderImage("sunes-timeline.jpg", "Timeline of important events")
            ];

    private static IReadOnlyCollection<ImageShowComponent.ShowImage> HingeImages = [
                HingeImage("sunday-mornings.webp", "Sunday mornings"),
                HingeImage("stratego.webp", "Stratego"),
                HingeImage("inner-language.webp", "Internal Language"),
                HingeImage("where-is-it.webp", "LaLandia"),
                HingeImage("ways-of-thinking.webp", "Ways of the Mind")
            ];
    private static IReadOnlyCollection<ImageShowComponent.ShowImage> FeeldImages = [
            TinderImage("tinder-frontpage.png", "Adventure"),
                TinderImage("sune-w-quote-1.png", "Jesper endorsement 1/3"),
                TinderImage("sune-w-quote-2.png", "Jesper endorsement 2/3"),
                TinderImage("sune-w-quote-3.png", "Jesper endorsement 3/3"),
                TinderImage("partner-in-crime.jpg", "Partner in crime"),
                TinderImage("sunes-timeline.jpg", "Timeline of important events")
        ];

    private static IReadOnlyCollection<ImageShowComponent.ShowImage> HappnImages = [
                HappnImage("when-a-man-loves-a-woman-1.jpg", "When a man loves a woman 1/3"),
                HappnImage("when-a-man-loves-a-woman-2.jpg", "When a man loves a woman 2/3"),
                HappnImage("when-a-man-loves-a-woman-3.jpg", "When a man loves a woman 3/3"),
                HappnImage("thinking-out-loud-conversation-topics.png", "Conversation topics"),
                HappnImage("books-i-like.jpg", "Books I like")
            ];

}
