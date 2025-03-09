namespace SuneDoes.UI.Pages.Blocks.Model;

public record BlocksChapter(
    string Title,
    IReadOnlyCollection<BlocksContent> Content
    )
{ 
}
