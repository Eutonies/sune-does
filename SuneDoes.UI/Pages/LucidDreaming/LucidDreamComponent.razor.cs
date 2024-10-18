using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.LucidDreaming;

public partial class LucidDreamComponent
{
    [Parameter]
    public DateTime DreamDate { get; set; }

    [Parameter]
    public RenderFragment TextPart { get; set; }


}
