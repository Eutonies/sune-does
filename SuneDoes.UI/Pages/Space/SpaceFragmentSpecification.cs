using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using SuneDoes.UI.Integration.Github;

namespace SuneDoes.UI.Pages.Space;

public record SpaceFragmentSpecification(
    string Name,
    string? SubName,
    Func<GitHubRepoFile, bool> FileFilter,
    Func<BookChapterContent, bool> ContentFilter
    )
{

    private static long _currentId = 0L;
    private static readonly object _idLock = new {};
    private static long NextId()
    {
        lock (_idLock)
        {
            _currentId += 1;
            return _currentId;
        }
    }
    public readonly long FragmentId = NextId();


    public static readonly IReadOnlyCollection<SpaceFragmentSpecification> Specifications = [
        new SpaceFragmentSpecification(
               Name: "Laura & Will",
               SubName: "Emotional Responsibility",
               FileFilter: (file) => file.FileContent.ToLower()
                  .Pipe(cont => cont.Contains("will") && cont.Contains("laura") && cont.Contains("emotion")),
               ContentFilter: _ => true
            )
        
        ];


}
