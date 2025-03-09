using Microsoft.AspNetCore.Html;
using SuneDoes.Extensions;

namespace SuneDoes.UI.Pages.Blocks.Model;

public abstract record TextContent(IReadOnlyCollection<BlockWord> EmphasisWords)
{
    public abstract HtmlString ToHtmlString();
    public abstract string StringContent();

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

    private IReadOnlyCollection<BlocksTextPart>? _parts;
    public IReadOnlyCollection<BlocksTextPart> Parts => _parts ??= SplitContent();

    private IReadOnlyCollection<BlocksTextPart> SplitContent()
    {
        var currentRound = new List<BlocksTextPart> {
            new BlocksTextPart(Text: StringContent())
        };
        foreach(var emphWord in EmphasisWords)
        {
            var nextRound = new List<BlocksTextPart>();
            foreach(var exist in currentRound)
            {
                if (exist.FontSettings != null || !exist.Text.Contains(emphWord.Words))
                    nextRound.Add(exist);
                else
                {
                    var splitted = exist.Text.Split(emphWord.Words);
                    foreach(var (part,indx) in splitted.Select((_,indx) => (_,indx)))
                    {
                        nextRound.Add(new BlocksTextPart(part));
                        if(indx < splitted.Length - 1)
                            nextRound.Add(new BlocksTextPart(
                                Text: emphWord.Words,
                                FontSettings: new BlocksFontSetting(
                                    FontFamily: emphWord.FontFamily,
                                    FontSize: emphWord.FontSize,
                                    FontWeight: emphWord.IsBold ? 900 : 500,
                                    FontColor: emphWord.Color))
                                );
                    }
                }
            }
            currentRound = nextRound;
        }

        return currentRound;
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
    public override string StringContent() => Content;
}

public record TextSpeakContent(string Content, IReadOnlyCollection<BlockWord> EmphasisWords) : TextContent(EmphasisWords)
{
    public override HtmlString ToHtmlString()
    {
        var returnee = new HtmlString($"<i>{Content}</i>");
        return returnee;
    }
    public override string StringContent() => Content;

}