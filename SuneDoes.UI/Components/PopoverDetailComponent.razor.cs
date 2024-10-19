using Microsoft.AspNetCore.Components;
using System.Text;

namespace SuneDoes.UI.Components;

public partial class PopoverDetailComponent
{
    [Parameter]
    public string? TextColor { get; set; }

    [Parameter]
    public string? BorderColor { get; set; }

    [Parameter]
    public string? ButtonBackgroundColor { get; set; }

    [Parameter]
    public string? PopoverBackgroundColor { get; set; }


    [Parameter]
    public string? FontFamily { get; set; }

    [Parameter]
    public int? FontSize { get; set; }

    [Parameter]
    public string ButtonText { get; set; }

    [Parameter]
    public string PopoverText { get; set; }


    private static long _currentId = 0;
    private static object _idLock = new object();
    private static long NextId()
    {
        lock (_idLock)
            return ++_currentId;
    }

    private readonly long _id = NextId();
    private string _stringId => $"sundo-popover-detailed-{_id}";



    private string BuildButtonStyleAttribute()
    {
        var returnee = new StringBuilder();
        returnee.Append(BuildGeneralStyleAttribute());
        if (ButtonBackgroundColor != null)
            returnee.Append($"background-color: {ButtonBackgroundColor}; ");
        return returnee.ToString();
    }

    private string BuildPopoverStyleAttribute()
    {
        var returnee = new StringBuilder();
        returnee.Append(BuildGeneralStyleAttribute());
        if (PopoverBackgroundColor != null)
            returnee.Append($"background-color: {PopoverBackgroundColor}; ");
        return returnee.ToString();
    }


    private StringBuilder BuildGeneralStyleAttribute()
    {
        var returnee = new StringBuilder();
        if (TextColor != null)
            returnee.Append($"color: {TextColor}; ");
        if (BorderColor != null)
            returnee.Append($"border-color: {BorderColor}; ");
        if (FontFamily != null)
            returnee.Append($"font-family: {FontFamily}; ");
        if (FontSize != null)
            returnee.Append($"font-size: {FontSize}; ");
        return returnee;

    }


}
