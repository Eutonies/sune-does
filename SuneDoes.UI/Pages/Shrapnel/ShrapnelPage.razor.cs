
using Malarkey.Abstractions.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Components;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Pages.Shrapnel.Model;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.Shrapnel;

public partial class ShrapnelPage
{
    private static IReadOnlyCollection<ShrapnelChapter> ShrapnelChapters = [];

    [Inject]
    public IOptions<SuneDoesConfiguration> Config { get; set; }

    private ShrapnelChapter? _currentChapter;

    private string? BackgroundImage => "images/shrapnel/" + ( 
        _currentChapter?.Pipe(pip => $"{(pip.Order < 10 ? "0" : "") + pip.Order}-sunset.webp") ??
        "unset.webp");


    private void OnChapterSelected(ShrapnelChapter chapter)
    {
        _currentChapter = chapter;
        InvokeAsync(StateHasChanged);
    }


    protected override Task OnParametersSetAsync()
    {
        CheckLoadShrapnel(Config.Value);
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
