
using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Pages.Shrapnel.Model;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.Shrapnel;

public partial class ShrapnelPage
{
    private static IReadOnlyCollection<ShrapnelChapter> ShrapnelChapters = [];

    [Inject]
    public SuneDoesConfiguration Config { get; set; }


    protected override Task OnParametersSetAsync()
    {
        CheckLoadShrapnel(Config);
        if(SessionState!= null)
        {
            SessionState.SelectedPage = SessionSelectedPage.Shrapnel;
        }
        return base.OnParametersSetAsync();
    }

    protected async override Task OnInitializedAsync()
    {
    }



    private static readonly object ShrapnelReadLock = new { };
    private static void CheckLoadShrapnel(SuneDoesConfiguration conf)
    {
        lock (ShrapnelReadLock)
        {
            if(!ShrapnelChapters.Any())
            {
                ShrapnelChapters = ShrapnelParser.ParseFolder(conf.ShrapnelFolder)
                    .OrderBy(_ => _.Order)
                    .ToList();
            }
        }
    }



}
