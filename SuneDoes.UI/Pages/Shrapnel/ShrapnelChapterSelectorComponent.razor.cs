using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Pages.Shrapnel.Model;

namespace SuneDoes.UI.Pages.Shrapnel;

public partial class ShrapnelChapterSelectorComponent
{
    [Parameter]
    public IReadOnlyCollection<ShrapnelChapter> Chapters { get; set; }

    [Parameter]
    public Action<ShrapnelChapter> OnChapterSelected { get; set; }


}
