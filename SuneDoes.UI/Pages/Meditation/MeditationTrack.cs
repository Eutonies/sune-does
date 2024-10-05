namespace SuneDoes.UI.Pages.Meditation;

public record MeditationTrack(
        string TrackId,
        string TrackName,
        string TrackUrl,
        string ButtonImage,
        IReadOnlyCollection<string> Summary,
        IReadOnlyCollection<string>? BreathingTechniques
    );
