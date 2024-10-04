using Microsoft.AspNetCore.Components;
using SuneDoes.Extensions;

namespace SuneDoes.UI.Pages.Meditation;

public partial class MeditationTracksComponent
{

    private static string ButtonImage(int no) => $"images/meditation/button-image-0{no}.webp";

    private static readonly IReadOnlyCollection<MeditationTrack> MeditationTracks = [
        new (
            TrackId: "1618123647", 
            TrackName: "The Garden", 
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/meditations-chapter01-the-garden", 
            ButtonImage: ButtonImage(1),
            Summary: [
                "You are introduced to the fundamental breathing techniques: Zen-breathing, Shotgun-breathing, and Uzi-breathing.",
                "Your first exploration of your happy place is disturbed by a filthy presence."
                ]),
        new (
            TrackId: "1621832136",
            TrackName: "The Forest",
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/meditations-chapter02-the-forest",
            ButtonImage: ButtonImage(2),
            Summary: [
                "You journey deeper into the depths of your consciousness and have a first class chance to meditate on the boundary between inter-human trust and self-sufficiency."
                ]),
        new (
            TrackId: "1627914372",
            TrackName: "The Tavern",
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/chapter03-the-tavern",
            ButtonImage: ButtonImage(3),
            Summary: [
                "From confronting the deep and hidden workings of your mind, you are thrust into meditations on the reflections of the outside world on your mind.",
                "And then you dance.",
                "And then, there are her eyes. Welcoming you."
                ]),
        new (
            TrackId: "1633424091",
            TrackName: "The High-rise",
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/chapter04-thehighrise",
            ButtonImage: ButtonImage(4),
            Summary: [
                "Sharpening your mind and honing your conscious thought allows you to be a productive and influencial part of society, but what to do with that productivity and what parts of society to influence?",
                "You have a chance to sky-rocket into the filthy rich 0.1%, but is it really all it's cracked up to be?",
                "How will you deal with the enemies you make across the way?"
                ]),
        new (
            TrackId: "1637704026",
            TrackName: "The Storm",
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/chapter05-thestorm",
            ButtonImage: ButtonImage(5),
            Summary: [
                "Some men cower or run in the face of a storm, while others will hold their ground. How will you deal with the inevitable Storm of Change?",
                "And there is glass!"
                ]),
        new (
            TrackId: "1650975804",
            TrackName: "The Slaughterhouse",
            TrackUrl: "https://soundcloud.com/sune-tzu-forkbeard/chapter06-theslaughterhouse",
            ButtonImage: ButtonImage(6),
            Summary: [
                "Some men will run straight into the storm, while others yet bring the storm.",
                "How will you fight pure evil in the storm-ridden wonderland of your mind?"
                ])

        ];


    private static MarkupString ButtonImageCssRules => MeditationTracks
        .Select((_, indx) => indx + 1)
        .Select(indx => $"#sundo-meditation-accordian-button-{indx} {{ background-image: url('images/meditation/button-image-0{indx}.webp);}}")
        .MakeString("\r\n")
        .Pipe(_ => new MarkupString(_));



    public record MeditationTrack(
        string TrackId,
        string TrackName,
        string TrackUrl,
        string ButtonImage,
        IReadOnlyCollection<string> Summary
        );

}
