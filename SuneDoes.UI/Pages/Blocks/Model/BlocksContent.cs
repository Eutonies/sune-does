using SuneDoes.Extensions;

namespace SuneDoes.UI.Pages.Blocks.Model;

public abstract record BlocksContent()
{


}


public record BlocksTextContent(IReadOnlyCollection<TextContent> Contents) : BlocksContent()
{
}

public record BlocksListContent(IReadOnlyCollection<(string? Title, string Content)> Items, bool IsOrdered) : BlocksContent()
{

}

public record BlocksNewLineContent() : BlocksContent();