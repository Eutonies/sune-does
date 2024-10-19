
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

}
