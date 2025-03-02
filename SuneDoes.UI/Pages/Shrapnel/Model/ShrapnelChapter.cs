namespace SuneDoes.UI.Pages.Shrapnel.Model;

public record ShrapnelChapter(
    string Name,
    int Order,
    IReadOnlyCollection<ShrapnelParagraph> Paragraphs
    );
