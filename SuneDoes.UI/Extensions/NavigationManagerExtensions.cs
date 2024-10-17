using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using SuneDoes.UI.Configuration;
using SuneDoes.UI.Pages.OnlineDating;
using System.Reflection;

namespace SuneDoes.UI.Extensions;

public static class NavigationManagerExtensions
{
    public static void NavigateTo<TPage>(this NavigationManager navManager, IOptions<SuneDoesConfiguration> options)
    {
        var url = typeof(OnlineDatingPage)
                    .GetCustomAttribute<RouteAttribute>()!
                    .Template;
        var basePath = options.Value?.HostingBasePath;
        if (basePath != null)
            navManager.NavigateTo("/" + basePath + url);
        else
            navManager.NavigateTo(url);
    }

}
