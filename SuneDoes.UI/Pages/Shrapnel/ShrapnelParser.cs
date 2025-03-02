using Microsoft.Extensions.FileSystemGlobbing;
using SuneDoes.Extensions;
using SuneDoes.UI.Pages.Shrapnel.Model;
using System.Text.RegularExpressions;

namespace SuneDoes.UI.Pages.Shrapnel;

public static class ShrapnelParser
{
    public static IReadOnlyCollection<ShrapnelChapter> ParseFolder(string folder)
    {
        var files = Directory.GetFiles(folder);
        var chapters = files
            .Where(_ => _.ToLower().EndsWith("shrapnel"))
            .Order()
            .Select(fil => ParseFileName(fil)
                .Pipe(pa =>
                    new ShrapnelChapter(
                        Name: pa.ChapterName,
                        Order: pa.ChapterOrder,
                        Paragraphs: Parse(File.ReadAllText(fil))
                ))
            )
            .OrderBy(_ => _.Order)
            .ToList();
        return chapters;
    }

    private static readonly Regex LineRegex = new Regex(@"\- *\(([a-z]+)\) * ""([^""]+)"" *(\[[^\n]+\])?", RegexOptions.IgnoreCase);
    private static readonly Regex ContinuedLineRegex = new Regex(@"""([^""]+)"" *(\[[^\n]+\])?", RegexOptions.IgnoreCase);

    private static (string ChapterName, int ChapterOrder) ParseFileName(string fileName) => Path.GetFileName(fileName)
        .Trim('0')
        .Pipe(str => str.Split('-'))
        .Pipe(pa => (
           ChapterName: pa[1].Replace(".shrapnel", ""), 
           ChapterOrder: int.Parse(pa[0])
        ));


    private static IReadOnlyCollection<ShrapnelParagraph> Parse(string fileContent)
    {
        var paragraphs = new List<ShrapnelParagraph>();
        var currentParagraph = new List<ShrapnelLine>();
        string? currentCharacter = null;
        foreach(var line in fileContent.Split("\n"))
        {
            var trimmed = line.Trim();
            if(trimmed.Length == 0) continue;
            if(trimmed.StartsWith("..."))
            {
                paragraphs.Add(new ShrapnelParagraph(currentParagraph));
                currentParagraph = new List<ShrapnelLine>();
                currentCharacter = null;
                continue;
            }
            var characterNamedMatches = LineRegex.Matches(trimmed);
            var unnamedMatches = ContinuedLineRegex.Matches(trimmed);
            if(characterNamedMatches.Any())
            {
                var match = characterNamedMatches.First();
                currentCharacter = match.Groups[1].Value;
                var spoken = match.Groups[2].Value;
                var description = match.Groups.Count > 3 ? match.Groups[3].Value : null;
                currentParagraph.Add(new ShrapnelLine(currentCharacter, spoken, string.IsNullOrWhiteSpace(description) ? null : description));
            }
            else if(currentCharacter != null && unnamedMatches != null)
            {
                var match = unnamedMatches.First();
                var spoken = match.Groups[1].Value;
                var description = match.Groups.Count > 2 ? match.Groups[2].Value : null;
                currentParagraph.Add(new ShrapnelLine(currentCharacter, spoken, string.IsNullOrWhiteSpace(description) ? null : description));

            }
        }
        if (currentParagraph.Count > 0)
            paragraphs.Add(new ShrapnelParagraph(currentParagraph));
        return paragraphs;

    }



}
