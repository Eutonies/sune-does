using Booktex.Domain.Book.Model;
using SuneDoes.UI.Integration.Github;

namespace SuneDoes.UI.Pages.Space;

public interface ISpaceParser
{

    Task<IReadOnlyCollection<BookChapterContent>> LoadContents(Func<GitHubRepoFile, bool> fileFilter);

}
