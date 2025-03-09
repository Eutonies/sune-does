
using Malarkey.Abstractions.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Components;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Pages.Blocks.Model;
using SuneDoes.UI.Pages.Shrapnel.Model;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.Blocks;

public partial class BlocksPage
{

    [Inject]
    public IOptions<SuneDoesConfiguration> Config { get; set; }

    private IReadOnlyCollection<BlocksChapter> AllChapters = [];

    private IReadOnlyCollection<ContentPage> Pages = [
        new ContentPage(0),
        new ContentPage(1),
        new ContentPage(2)
        ];


    private void OnChapterSelected(ShrapnelChapter chapter)
    {
        InvokeAsync(StateHasChanged);
    }

    protected override Task OnInitializedAsync()
    {
        AllChapters = BlocksParser.LoadChapters(Config.Value.BlocksFolder);
        return base.OnInitializedAsync();
    }


    protected override Task OnParametersSetAsync()
    {
        if(SessionState!= null)
        {
            SessionState.SelectedPage = SessionSelectedPage.Blocks;
        }
        return base.OnParametersSetAsync();
    }


    private record ContentPage(
        int Index
        );


}
