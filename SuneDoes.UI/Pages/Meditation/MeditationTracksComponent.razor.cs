namespace SuneDoes.UI.Pages.Meditation;

public partial class MeditationTracksComponent
{
    private static readonly IReadOnlyCollection<MeditationTrack> MeditationTracks = [
        new (
            TrackId: "1618123647", 
            TrackName: "The Garden", 
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/meditations-chapter01-the-garden", 
            Summary: [
                "You are introduced to the fundamental breathing techniques: Zen-breathing, Shotgun-breathing, and Uzi-breathing.",
                "Your first exploration of your happy place is disturbed by a filthy presence."
                ]),
        new (
            TrackId: "1621832136",
            TrackName: "The Forest",
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/meditations-chapter02-the-forest",
            Summary: []),
        new (
            TrackId: "1627914372",
            TrackName: "The Tavern",
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/chapter03-the-tavern",
            Summary: []),
        new (
            TrackId: "1633424091",
            TrackName: "The High-rise",
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/chapter04-thehighrise",
            Summary: []),
        new (
            TrackId: "1637704026",
            TrackName: "The Storm",
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/chapter05-thestorm",
            Summary: []),
        new (
            TrackId: "1650975804",
            TrackName: "The Slaughterhouse",
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/chapter06-theslaughterhouse",
            Summary: [])

        ];




    public record MeditationTrack(
        string TrackId,
        string TrackName,
        string TrackUrl,
        IReadOnlyCollection<string> Summary
        );

}
