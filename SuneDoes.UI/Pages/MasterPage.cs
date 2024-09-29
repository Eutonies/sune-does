using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages;

public class MasterPage : ComponentBase
{
    [CascadingParameter]
    public SessionState SessionState { get; set; }


}
