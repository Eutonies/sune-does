using Booktex.Domain.Book.Model;
using Booktex.Domain.Parsing;
using SuneDoes.UI.Integration.Github;

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
        var parsed = WritingParser.ParseFileContents(relFile.FileContent);
        return parsed;
    }



}
