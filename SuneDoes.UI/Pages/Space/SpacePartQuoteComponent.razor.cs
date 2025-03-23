using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.Space;

public partial class SpacePartQuoteComponent
{
    [Parameter]
    public BookQuote Quote { get; set; }

}
