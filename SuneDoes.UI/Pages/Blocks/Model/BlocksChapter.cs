namespace SuneDoes.UI.Pages.Blocks.Model;

public record BlocksChapter(
    string Title,
    string Order,
    IReadOnlyCollection<BlocksContent> Content
    )
{ 
}
