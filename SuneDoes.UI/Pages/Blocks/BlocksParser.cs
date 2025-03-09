using Malarkey.Abstractions.Util;
using SuneDoes.UI.Pages.Blocks.Model;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SuneDoes.UI.Pages.Blocks;

public static class BlocksParser
{
    private static IReadOnlyCollection<BlocksChapter>? Chapters;
    private static IReadOnlyDictionary<string, string> TitleMap = new List<(string In, string Out)>
    {
        ("the-forest", "The Forest"),
        ("the-village", "The Village"),
        ("the-whore", "The Whore")
    }.ToDictionarySafe(_ => _.In, _ => _.Out);


    public static IReadOnlyCollection<BlocksChapter> LoadChapters(string blocksFolder)
    {
        Chapters ??=  ParseChapters(blocksFolder);
        return Chapters;
    }


    private static IReadOnlyCollection<BlockWord> ParseBlockWords(string blocksFolder)
    {
        var fileContents = Directory.GetFiles(blocksFolder)
            .Where(_ => _.ToLower().EndsWith(".words"))
            .Select(File.ReadAllText)
            .ToList();

        var returnee = fileContents
            .SelectMany(cont => JsonSerializer.Deserialize<IReadOnlyCollection<BlockWord>>(cont)!)
            .DistinctBy(_ => _.Words)
            .ToList();
        return returnee;
    }

    private static readonly Regex FileNameRegex = new Regex(@"([0-9]+)\-(.*)\.block", RegexOptions.IgnoreCase);


    private static IReadOnlyCollection<BlocksChapter> ParseChapters(string blocksFolder)
    {
        var emphWords = ParseBlockWords(blocksFolder);
        var relevantFiles = Directory.GetFiles(blocksFolder)
            .Where(_ => _.ToLower().EndsWith(".block"))
            .ToList();
        var returnee = relevantFiles
            .Select(_ => ParseChapterFile(_, emphWords))
            .OrderBy(_ => _.Order)
            .ToList();
        return returnee;

    }


    private static BlocksChapter ParseChapterFile(string fileName, IReadOnlyCollection<BlockWord> emphasisWords)
    {
        var match = FileNameRegex.Matches(fileName).First();
        var order = match.Groups[1].Value;
        var title = match.Groups[2].Value;
        title = TitleMap.GetValueOrDefault(title.ToLower()) ?? title;
        var fileContent = File.ReadAllText(fileName);
        var blocks = ParseFile(fileContent, emphasisWords);
        var returnee = new BlocksChapter(title, order, blocks);
        return returnee;
    }


    private static IReadOnlyCollection<BlocksContent> ParseFile(string fileContent, IReadOnlyCollection<BlockWord> emphasisWords)
    {
        var returnee = new List<BlocksContent>();
        var currentItems = new List<string>();
        var currentTextContents = new List<TextContent>();
        var (startIndex, endIndex, isSpeach, newLine, isItemList) = (0,0, false, true, false);

        var EndItemList = () =>
        {
            if(currentItems.Any())
            {
                var insertee = new BlocksListContent(Items: currentItems.Select(it => (Title: (string?)null, Content: it.StripItemPrefix())).ToList(), IsOrdered: true);
                returnee.Add(insertee);
            }
            currentItems.Clear();
        };

        var EndTextContents = () =>
        {
            if (currentTextContents.Any())
                returnee.Add(new BlocksTextContent(currentTextContents.ToList()));
            currentTextContents.Clear();
        };

        var EndCurrent = () =>
        {
            var length = endIndex - startIndex;
            string? curString = 
                endIndex > startIndex ? 
                fileContent.Substring(startIndex, length)
                   .Trim('\n')
                   .Trim('\r')
                   .Pipe(_ => _.Trim())
                   .Pipe(_ => _.Length == 0 ? null : _) : 
                null;
            if (curString == null)
                return;
            if(isItemList)
                currentItems.Add(curString);
            else if(isSpeach)
                currentTextContents.Add(new TextSpeakContent(curString, emphasisWords));
            else 
                currentTextContents.Add(new TextNarrationContent(curString, emphasisWords));
            startIndex = endIndex;
                
        };

        for(; endIndex < fileContent.Length; endIndex++)
        {
            var curChar = fileContent[endIndex];
            if (curChar.IsCarriageReturn())
            {
                newLine = false;
                continue;
            }
            else if(curChar == '1' && newLine && !isSpeach && !isItemList)
            {
                EndCurrent();
                isItemList = true;
            }
            else if(curChar.IsInt() && newLine && isItemList)
            {
                EndCurrent();
            }
            else if(!curChar.IsInt() && newLine && isItemList)
            {
                EndItemList();
                isItemList = false;
            }
            else if(curChar.IsNewline() && isItemList)
            {
                EndCurrent();
            }
            else if(curChar.IsNewline() && !isSpeach)
            {
                EndCurrent();
                EndTextContents();
                returnee.Add(new BlocksNewLineContent());
            }
            else if (curChar == '"')
            {
                EndCurrent();
                isSpeach = !isSpeach;
            }
            newLine = curChar.IsNewline();

        }
        if (startIndex < endIndex)
            EndCurrent();
        if (currentItems.Any())
            EndItemList();
        if (currentTextContents.Any())
            EndTextContents();

        return returnee;
    }

    private static bool IsInt(this char c) => IntegerChars.Contains(c);
    private static bool IsNewline(this char c) => c == '\n';
    private static bool IsCarriageReturn(this char c) => c == '\r';

    private static HashSet<char> CharSet(params IEnumerable<char> chars) => chars.ToHashSet();
    private static readonly HashSet<char> IntegerChars = CharSet('1', '2', '3', '4', '5', '6', '7', '8', '9');

    private static readonly Regex ItemStripRegex = new Regex(@"[0-9]+\. ?(.*)");
    private static string StripItemPrefix(this string str) => ItemStripRegex.Matches(str).First().Groups[1].Value;

}
