using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.Space;

public partial class SpacePartSingingComponent
{
    [Parameter]
    public BookSinging Singing { get; set; }

    private IReadOnlyCollection<(string Line, int Index)> AllLines => Singing
        .LinesSong
        .Select((_,indx) => (Line: _, Index: indx))
        .ToReadonlyCollection(); 

}
