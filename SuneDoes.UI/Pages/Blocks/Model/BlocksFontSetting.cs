using Microsoft.AspNetCore.Html;
using System.Text;

namespace SuneDoes.UI.Pages.Blocks.Model;

public record BlocksFontSetting(
    string? FontFamily,
    int? FontSize,
    int? FontWeight,
    string? FontColor
    )
{
    public string ToHtml()
    {
        var sb = new StringBuilder();
        if( FontFamily != null )
            sb.Append( $"font-family: {FontFamily}; ");
        if (FontSize != null)
            sb.Append($"font-size: {FontSize}; ");
        if (FontWeight != null)
            sb.Append($"font-weight: {FontWeight}; ");
        if (FontColor != null)
            sb.Append($"color: {FontColor}; ");
        return sb.ToString();
    }
}
