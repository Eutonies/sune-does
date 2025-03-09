
using Malarkey.Abstractions.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Components;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Pages.Shrapnel.Model;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.Blocks;

public partial class BlocksPage
{

    [Inject]
    public IOptions<SuneDoesConfiguration> Config { get; set; }


    private void OnChapterSelected(ShrapnelChapter chapter)
    {
        InvokeAsync(StateHasChanged);
    }


    protected override Task OnParametersSetAsync()
    {
        if(SessionState!= null)
        {
            SessionState.SelectedPage = SessionSelectedPage.Shrapnel;
        }
        return base.OnParametersSetAsync();
    }




}
