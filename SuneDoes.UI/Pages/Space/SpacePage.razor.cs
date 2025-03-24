
using Malarkey.Abstractions.Util;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Components;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Integration.Github;
using SuneDoes.UI.Pages.Shrapnel;
using SuneDoes.UI.Pages.Shrapnel.Model;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.Space;

public partial class SpacePage
{
    private static readonly SemaphoreSlim FragmentReadLock = new SemaphoreSlim(1,1);
    private static readonly IReadOnlyCollection<FragmentCacheRecord> Cache = SpaceFragmentSpecification.Specifications
        .Select(_ => new FragmentCacheRecord(_))
        .ToList();
    private static DateTime LastCacheUpdate = DateTime.Now;

    [Inject]
    public ISpaceParser SpaceParser { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await CheckReadFragments(SpaceParser);
        if (SessionState != null && SessionState.SelectedPage != SessionSelectedPage.SpaceAndTime)
        {
            SessionState.SelectedPage = SessionSelectedPage.SpaceAndTime;
            await InvokeAsync(StateHasChanged);
        }
    }

    private void OnNoteClicked(SpaceFragmentSpecification spec, SpaceFragment frag)
    {
        _recordToDisplay = Cache
            .FirstOrDefault(_ => _.Specification.FragmentId == spec.FragmentId);
        _ = InvokeAsync(StateHasChanged);
    }

    private void OnCloseClicked()
    {
        _recordToDisplay = null;
        _ = InvokeAsync(StateHasChanged);
    }

    private void OnRefreshCache()
    {
        if ((DateTime.Now - LastCacheUpdate).TotalSeconds < 5)
            return;
        Task.Run(async () =>
        {
            await FragmentReadLock.WaitAsync();
            try
            {
                foreach (var record in Cache)
                    record.Fragment = null;
                LastCacheUpdate = DateTime.Now;
            }
            finally
            {
                FragmentReadLock.Release();
            }
            await CheckReadFragments(SpaceParser);

        });
    }

    private FragmentCacheRecord? _recordToDisplay;


    private static async Task CheckReadFragments(ISpaceParser spaceParser)
    {
        if (!Cache.Any(_ => _.Fragment == null))
            return;
        await FragmentReadLock.WaitAsync();
        try
        {
            if (!Cache.Any(_ => _.Fragment == null))
                return;
            var notLoaded = Cache
                .Where(_ => _.Fragment == null)
                .ToList();
            foreach(var cac in notLoaded)
            {
                var loaded = await spaceParser.LoadContents(cac.Specification.FileFilter);
                var filtered = cac.Specification.ContentFilter(loaded);
                if (filtered.Count > 0)
                    cac.Fragment = new SpaceFragment(cac.Specification, filtered);
            }
        }
        finally
        {
            FragmentReadLock.Release();
        }
        
    }



    private record FragmentCacheRecord(
        SpaceFragmentSpecification Specification
        )
    {
        public SpaceFragment? Fragment { get; set; }
    }


}
