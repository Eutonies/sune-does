using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using SuneDoes.UI.Integration.Github;

namespace SuneDoes.UI.Pages.Space;

public record SpaceFragmentSpecification(
    string Name,
    string? SubName,
    Func<GitHubRepoFile, bool> FileFilter,
    Func<IReadOnlyCollection<BookChapterContent>, IReadOnlyCollection<BookChapterContent>> ContentFilter
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
               SubName: "in The Pit",
               FileFilter: (file) => file.FileContent.ToLower()
                  .Pipe(cont => cont.Contains("will") && cont.Contains("laura") && cont.Contains("wham")),
               ContentFilter: input => input
        ),

        new SpaceFragmentSpecification(
               Name: "Laura & Will",
               SubName: "Emotional Responsibility",
               FileFilter: (file) => file.FileContent.ToLower()
                  .Pipe(cont => cont.Contains("will") && cont.Contains("laura") && cont.Contains("emotion")),
               ContentFilter: input => {
                   var returnee = new List<BookChapterContent>();
                   var startTaking = false;
                   foreach(var cont in input) {
                       if(cont is BookDialog diag) 
                       {
                           if(!startTaking && diag.Entries.Any(_ => _.Line.LineParts.Any(_ => _.PartText.ToLower().Contains("emotional responsibility"))))
                               startTaking = true;
                       }
                       if(startTaking) {
                           returnee.Add(cont);
                           if(returnee.Count == 3)
                               break;
                       }

                   }
                   return returnee;
               }
            )
        
        ];


}
