using Booktex.Domain.Book.Model;
using Booktex.Domain.Parsing;
using Microsoft.Extensions.FileSystemGlobbing;
using SuneDoes.Extensions;
using SuneDoes.UI.Integration.Github;
using SuneDoes.UI.Pages.Shrapnel.Model;
using System.Text;
using System.Text.RegularExpressions;

namespace SuneDoes.UI.Pages.Shrapnel;

public static class ShrapnelParser
{
    private static readonly string[] ChapterNames = [
        "Nights I can't forget",
        "Days where we came close",
        "The day I told you I love you",
        "Sleepless nights",
        "The worst days",
        "Grey days",
        "Nights with friends",
        "The day I broke it",
        "The day it broke me"
        ];

    public static async Task<IReadOnlyCollection<ShrapnelChapter>> ParseGitHub(IGitHubRepoBrowser browser)
    {
        var repoFiles = await browser.DownloadFilesWithEnding(repoName: "jen-and-will", ".shrapnel");
        var chapters = new List<ShrapnelChapter>();
        foreach(var file in repoFiles.OrderBy(_ => _.FileName))
        {
            Console.WriteLine($"Doing: {file.FileName}");
            var chapterContents = WritingParser.ParseFileContents(FixNewLines(file.FileContent));
            var (chapterName, chapterIndex) = ParseFileName(file.FileName);
            var chapter = new ShrapnelChapter(
                Name: chapterName,
                Order: chapterIndex,
                Paragraphs: chapterContents
                   .OfType<BookDialog>()
                   .Select(diag =>
                      new ShrapnelParagraph(
                          Lines: diag.Entries
                             .SelectMany(ent => ent.Line.LineParts.Select(_ => (Entry: ent, LinePart: _)))
                             .Select(comb => new ShrapnelLine(SaidBy: comb.Entry.Line.Character.CharacterName, Line: comb.LinePart.PartText, Description: comb.LinePart.Description))
                             .ToReadonlyCollection()
                       )
                   ).ToReadonlyCollection()
                );
            chapters.Add(chapter);

        }
        chapters = chapters
            .Select(_ => _ with
            {
                Name = _.Order - 1 < ChapterNames.Length ? ChapterNames[_.Order - 1] : _.Name
            })
            .OrderBy(_ => _.Order)
            .ToList();
        return chapters;
    }

    private static string FixNewLines(string str)
    {
        var returnee = new StringBuilder();
        var split = str.Split("\n");
        foreach (var part in split)
        {
            if (returnee.Length > 0)
            {
                if (returnee[returnee.Length - 1] != '\r')
                    returnee.Append('\r');
                returnee.Append('\n');
            }
            foreach (var ch in part)
            {
                returnee.Append(ch);
            }
        }
        return returnee.ToString();
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
                description = description?.Pipe(desc => desc.Replace("[", "").Replace("]", ""));
                currentParagraph.Add(new ShrapnelLine(currentCharacter, spoken, string.IsNullOrWhiteSpace(description) ? null : description));
            }
            else if(currentCharacter != null && unnamedMatches != null)
            {
                var match = unnamedMatches.First();
                var spoken = match.Groups[1].Value;
                var description = match.Groups.Count > 2 ? match.Groups[2].Value : null;
                description = description?.Pipe(desc => desc.Replace("[", "").Replace("]", ""));
                currentParagraph.Add(new ShrapnelLine(currentCharacter, spoken, string.IsNullOrWhiteSpace(description) ? null : description));

            }
        }
        if (currentParagraph.Count > 0)
            paragraphs.Add(new ShrapnelParagraph(currentParagraph));
        return paragraphs;

    }



}
