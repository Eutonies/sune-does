
using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
using SuneDoes.UI.Session;
using System.Globalization;

namespace SuneDoes.UI.Pages.LucidDreaming;

public partial class LucidDreamingPage
{
    private static readonly CultureInfo _daDk = CultureInfo.CreateSpecificCulture("da-DK");
    private static readonly Random Random = new Random();


    private object _glimmerLock = new { };
    private IReadOnlyCollection<Glimmer> _glimmers = new List<Glimmer>();

    private const int MinNumberOfSeconds = 15;
    private const int MaxNumberOfSeconds = 35;
    private static int DrawSeconds => MinNumberOfSeconds + Random.Next(0, MaxNumberOfSeconds - MinNumberOfSeconds);
    private static int DrawDelay => Random.Next(0, MaxNumberOfSeconds);

    private const int NumberOfGlimmers = 300;

    protected override Task OnParametersSetAsync()
    {
        if(SessionState!= null)
        {
            SessionState.SelectedPage = Session.SessionSelectedPage.LucidDreaming;
        }
        return base.OnParametersSetAsync();
    }

    protected override Task OnInitializedAsync()
    {
        _ = InitiateGlimmers();
        return base.OnInitializedAsync();
    }

    private DateTime D(string dateString) => DateTime.ParseExact(dateString, "dd-MM-yyyy", _daDk);

    private async Task InitiateGlimmers()
    {
        int? secondsToWait = null;
        lock (_glimmerLock)
        {
            if (_glimmers.Any())
                return;
            var stepBy = 1;
            var newGlimmers = new List<Glimmer>();
            for (var perc = 0; perc < 100; perc += stepBy)
            {
                for(var indx = 0; indx < NumberOfGlimmers / 100; indx++)
                {
                    var numberOfSeconds = DrawSeconds;
                    var delay = DrawDelay;
                    newGlimmers.Add(new Glimmer(perc, numberOfSeconds, delay));
                }
            }
            _glimmers = newGlimmers;
            secondsToWait = newGlimmers.Max(_ => _.InitialDelay + _.AnimationTimeSeconds) + 2;
        }
        await InvokeAsync(StateHasChanged);
        if(secondsToWait != null)
        {
            await Task.Delay(TimeSpan.FromSeconds(secondsToWait.Value));
            _glimmers = [];
            await InvokeAsync(StateHasChanged);
            await Task.Delay(TimeSpan.FromSeconds(1));
            _ = Task.Run(InitiateGlimmers);
        }



    }


    private record Glimmer(int PercentLeft, int AnimationTimeSeconds, int InitialDelay);

    private const string Det0201YoungWoman = @"She is actually referred to as 'a young woman' and the term is hence not used to anonymize anyone here.";

    private const string Det0201RomanceEnsures = @"It only seems fair to clarify, that the 'romance' in question was between myself and the young woman. If I am not mistaken, we tried to complete sexual intercourse 
in a queen-size bed mounted on 4 100m wooden poles that gave sway to our every movement. Our mating-attempt was unsuccesful.";

    private const string Det0601Lotte = @"The name 'Lotte' actually appears here which strikes me as weird, since I don't think I have ever had an even remotely close relation to anyone of that name.";

    private const string Det0801TAP = @"I have googled it, and I don't think the local shopping mall: 'Ballerup Centret' has ever had a restaurant of this name";

    private const string Det0801HerlevKoll = @"Both she and I have had residency in the dorm: 'Herlev Kollegiet', so this part may not be as random as it appears at first. The fact that bus route 147 
doesn't come anywhere near Herlev Kollegiet can easily be chalked up to me never using public transportation. 
";

    private const string Det1001Jacq = @"Pseudonym for a girl I went to high school with.";

    private const string Det1001MouhGoon = @"Translates to something like 'Martial Arts Hall', and refers to the place where I had been practicing South Chinese Kong Fu for about 6 years at the time of the dream.";

    private const string Det1701SingleLiving = @"There are pro's and con's to single-living. Not having to worry that you've punched someone while aggresively throwing fists across the bed is a definite pro!";

    private const string Det3001AngryXmas = @"From the way I have it written down, it's unclear to me if I was angry at the rivalling gangsters for intruding on our family Christmas celebration,
or if my mood was ruined by their lack of social grace in leaving before the main dish was served. If pressed, I would have to go with the former but I'm really not sure.";

    private const string Det0202Soccer = @"On a daily basis, I dedicate about 0.0% of my mental processes on soccer/football, and I really can't give any reasonable explanation for why soccer/football 
matches are part of the scenery of such a relatively large quantity of my dreams during this period.";
}


public class LucidPopover : PopoverDetailComponent
{
    public LucidPopover()
    {
        ButtonBackgroundColor = "rgba(250, 240, 240,0.6)";
        PopoverBackgroundColor = "rgba(250, 230, 230,0.9)";
        TextColor = "rgb(40, 14, 14)";
        FontFamily = "Merienda";
        FontSize = 12;
        BorderColor = "rgba(160, 61, 61,0.6)";

    }
}
