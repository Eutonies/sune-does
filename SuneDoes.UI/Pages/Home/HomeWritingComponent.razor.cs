using Microsoft.AspNetCore.Components;

namespace SuneDoes.UI.Pages.Home;

public partial class HomeWritingComponent
{
    [Parameter]
    public string Title { get; set; }

    [Parameter]
    public RenderFragment BodyFragment { get; set; }

    [Parameter]
    public RenderFragment FooterFragment { get; set; }

}
