
using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
using SuneDoes.UI.Session;
using System.Globalization;

namespace SuneDoes.UI.Pages.LucidDreaming;

public partial class LucidDreamingPage
{
    private readonly CultureInfo _daDk = CultureInfo.CreateSpecificCulture("da-DK");
    protected override Task OnParametersSetAsync()
    {
        if(SessionState!= null)
        {
            SessionState.SelectedPage = Session.SessionSelectedPage.OnlineDating;
        }
        return base.OnParametersSetAsync();
    }

    protected override Task OnInitializedAsync()
    {
        if(SessionState.CurrentShowImages == null)
        {
            
        }
        return Task.CompletedTask;
    }

    private DateTime D(string dateString) => DateTime.ParseExact(dateString, "dd-MM-yyyy", _daDk);


}
