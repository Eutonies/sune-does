using Microsoft.AspNetCore.Html;
using SuneDoes.Extensions;

namespace SuneDoes.UI.Pages.Blocks.Model;

public abstract record TextContent(IReadOnlyCollection<BlockWord> EmphasisWords)
{
    public abstract HtmlString ToHtmlString();

    protected string Replace(string input)
    {
        var returnee = EmphasisWords
            .Aggregate(input, (string curStat, BlockWord emphWord) =>
            {
                var replacement = $"<span style=\"{emphWord.Color?.Pipe(col => $"color: {col};")}\">{emphWord.Words}</span>";
                if (emphWord.IsBold)
                    replacement = $"<b>{replacement}</b>";
                if (emphWord.IsBold)
                    replacement = $"<i>{replacement}</i>";
                var replaced = curStat.Replace(emphWord.Words, replacement);
                return replaced;
            });
        return returnee;

    }

}

public record TextNarrationContent(string Content, IReadOnlyCollection<BlockWord> EmphasisWords
) : TextContent(EmphasisWords)
{
    public override HtmlString ToHtmlString()
    {
        var content = Replace(Content);
        return new HtmlString(content);
    }
}

public record TextSpeakContent(string Content, IReadOnlyCollection<BlockWord> EmphasisWords) : TextContent(EmphasisWords)
{
    public override HtmlString ToHtmlString()
    {
        var returnee = new HtmlString($"<i>{Content}</i>");
        return returnee;
    }
}