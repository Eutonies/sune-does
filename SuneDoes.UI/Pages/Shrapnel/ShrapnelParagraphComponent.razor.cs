using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Pages.Shrapnel.Model;

namespace SuneDoes.UI.Pages.Shrapnel;

public partial class ShrapnelParagraphComponent
{

    [Parameter]
    public ShrapnelParagraph Paragraph { get; set; }

    [Parameter]
    public int ParagraphOrder { get; set; }

    private bool IsEven => ParagraphOrder % 2 == 0;

}
