using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Pages.Shrapnel.Model;

namespace SuneDoes.UI.Pages.Shrapnel;

public partial class ShrapnelParagraphComponent
{
    [Inject]
    public ShrapnelParagraph Paragraph { get; set; }

}
