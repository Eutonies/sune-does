using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.Space;

public partial class SpacePartCharacterLineComponent
{
    [Parameter]
    public BookCharacterLine Line { get; set; }

    [Parameter]
    public bool IsRight { get; set; }



}
