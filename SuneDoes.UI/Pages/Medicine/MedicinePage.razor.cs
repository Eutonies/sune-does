
using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
using SuneDoes.UI.Components.Email;
using SuneDoes.UI.Session;
using System.Globalization;

namespace SuneDoes.UI.Pages.Medicine;

public partial class MedicinePage
{
    private bool _showDialog = true;
    private string _notifyMedicineType = "Heroin";

    private void CloseNotifyDialog()
    {
        _showDialog = false;
        _ = InvokeAsync(StateHasChanged);
    }

}


