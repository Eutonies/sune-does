using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.OnlineDating;

public partial class OnlineDatingAppComponent
{
    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public RenderFragment Text { get; set; }

    [Parameter]
    public RenderFragment AppLogo { get; set; }


}
