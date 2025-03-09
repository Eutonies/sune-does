using Malarkey.Abstractions.Util;
using SuneDoes.UI.Pages.Blocks.Model;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SuneDoes.UI.Pages.Blocks;

public static class BlocksParser
{

    public static IReadOnlyCollection<BlockWord> ParseBlockWords(string blocksFolder)
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


    public static IReadOnlyCollection<BlocksChapter> ParseChapters(string blocksFolder)
    {
        var emphWords = ParseBlockWords(blocksFolder);
        var relevantFiles = Directory.GetFiles(blocksFolder)
            .Where(_ => _.ToLower().EndsWith(".block"))
            .ToList();
    }


    private static BlocksChapter ParseChapterFile(string fileName, IReadOnlyCollection<BlockWord> emphasisWords)
    {
        var match = FileNameRegex.Matches(fileName).First();
        var order = match.Groups[1].Value;
        var title = match.Groups[2].Value;
        var fileContent = File.ReadAllText(fileName);

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
                
        };

        for(; endIndex < fileContent.Length; endIndex++)
        {
            var length = endIndex - startIndex;
            var curChar = fileContent[endIndex];
            char? nextChar = endIndex < fileContent.Length - 1 ? fileContent[endIndex + 1] : null;
            string? curString = endIndex > startIndex ? fileContent.Substring(startIndex, length).Trim('\n').Trim('\r') : null;
            if (curChar == '"')
            {
                if(isSpeach)
                {
                    var sentence = fileContent.Substring(startIndex, length);
                    (isSpeach, startIndex, newLine) = (false, endIndex + 1, false);
                }
                else
                {
                    (isSpeach, startIndex, newLine) = (true, endIndex, false);
                }
            }
            if(curChar == '1' && nextChar == '.' && newLine && !isSpeach && !isItemList)
            {
                isItemList = true;
            }
            else if(curChar.IsNewline() && isItemList)
            {
                if(curString != null)
                    currentItems.Add(curString);
            }
            else if(newLine && !curChar.IsInt() && isItemList)
            {
                if(currentItems.Any())
                {
                }
            }

        }

        return returnee;
    }

    private static bool IsInt(this char c) => IntegerChars.Contains(c);
    private static bool IsNewline(this char c) => NewLineChars.Contains(c);

    private static HashSet<char> CharSet(params IEnumerable<char> chars) => chars.ToHashSet();
    private static readonly HashSet<char> IntegerChars = CharSet('1', '2', '3', '4', '5', '6', '7', '8', '9');
    private static readonly HashSet<char> NewLineChars = CharSet('\r', '\n');

    private static readonly Regex ItemStripRegex = new Regex(@"[0-9]+\. ?(.*)");
    private static string StripItemPrefix(this string str) => ItemStripRegex.Matches(str).First().Groups[1].Value;

}
