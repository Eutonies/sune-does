using Booktex.Domain.Book.Model;
using Booktex.Domain.Parsing;
using SuneDoes.UI.Integration.Github;
using System.Text;
using System.Text.RegularExpressions;

namespace SuneDoes.UI.Pages.Space;

public class SpaceParser : ISpaceParser
{
    private readonly IGitHubRepoBrowser _browser;

    public SpaceParser(IGitHubRepoBrowser browser)
    {
        _browser = browser;
    }

    public async Task<IReadOnlyCollection<BookChapterContent>> LoadContents(Func<GitHubRepoFile, bool> fileFilter)
    {
        var allFiles = await _browser.DownloadFilesWithEnding("jen-and-will", ".story");
        var relFile = allFiles
            .FirstOrDefault(fileFilter);
        if (relFile == null)
            return [];
        relFile = relFile with
        {
            FileContent = FixNewLines(relFile.FileContent)
        };
        var parsed = WritingParser.ParseFileContents(relFile.FileContent);
        return parsed;
    }

    private static string FixNewLines(string str)
    {
        var returnee = new StringBuilder();
        var split = str.Split("\n");
        foreach(var part in split)
        {
            if(returnee.Length > 0)
            {
                if (returnee[returnee.Length - 1] != '\r')
                    returnee.Append('\r');
                returnee.Append('\n');
            }
            foreach(var ch in part)
            {
                returnee.Append(ch);
            }
        }
        return returnee.ToString();
    }

}
