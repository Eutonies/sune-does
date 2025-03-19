
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

    [Inject]
    public ISpaceParser SpaceParser { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await CheckReadFragments(SpaceParser);
    }


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
                if (loaded.Count > 0)
                    cac.Fragment = new SpaceFragment(cac.Specification, loaded);
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
