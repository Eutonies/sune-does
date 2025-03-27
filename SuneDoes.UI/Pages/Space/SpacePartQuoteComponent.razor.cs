using Booktex.Domain.Book.Model;
using Booktex.Domain.Util;
using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.Space;

public partial class SpacePartQuoteComponent
{
    [Parameter]
    public BookQuote Quote { get; set; }

    private string? QuoteClass => (Quote.Name?.ToLower()?.Trim(), Quote.SubName?.ToLower()?.Trim()) switch
    {
        (string nam, _) when nam.Contains("cold war") => "quot-cold-war",
        _ => null
    };


}
