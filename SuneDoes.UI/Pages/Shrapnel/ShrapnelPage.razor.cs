
using Malarkey.Abstractions.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Components;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Integration.Github;
using SuneDoes.UI.Pages.Shrapnel.Model;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.Shrapnel;

public partial class ShrapnelPage
{
    private static IReadOnlyCollection<ShrapnelChapter> ShrapnelChapters = [];

    [Inject]
    public IOptions<SuneDoesConfiguration> Config { get; set; }

    [Inject]
    public IGitHubRepoBrowser GitHubBrowser { get; set; }

    private ShrapnelChapter? _currentChapter;

    private string? BackgroundImage => "images/shrapnel/" + ( 
        _currentChapter?.Pipe(pip => $"{(pip.Order < 10 ? "0" : "") + pip.Order}-sunset.webp") ??
        "unset.webp");


    private void OnChapterSelected(ShrapnelChapter chapter)
    {
        _currentChapter = chapter;
        InvokeAsync(StateHasChanged);
    }


    protected override async Task OnParametersSetAsync()
    {
        await CheckLoadShrapnel(Config.Value, GitHubBrowser);
        if(SessionState!= null)
        {
            SessionState.SelectedPage = SessionSelectedPage.Shrapnel;
        }
    }



    protected async override Task OnInitializedAsync()
    {
    }



    private static readonly SemaphoreSlim ShrapnelReadLock = new SemaphoreSlim(1,1);
    private static async Task CheckLoadShrapnel(SuneDoesConfiguration conf, IGitHubRepoBrowser browser)
    {
        await ShrapnelReadLock.WaitAsync();
        try
        {
            if (!ShrapnelChapters.Any())
            {
                ShrapnelChapters = (await ShrapnelParser.ParseGitHub(browser))
                    .OrderBy(_ => _.Order)
                    .ToList();
            }

        }
        finally
        {
            ShrapnelReadLock.Release();
        }

    }



}
