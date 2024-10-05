using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.Meditation;

public partial class MeditationTrackComponent
{
    [Parameter]
    public MeditationTrack Track { get; set; }
}
