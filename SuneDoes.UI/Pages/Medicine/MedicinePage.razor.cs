
using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
using SuneDoes.UI.Components.Email;
using SuneDoes.UI.Session;
using System.Globalization;

namespace SuneDoes.UI.Pages.Medicine;

public partial class MedicinePage
{
    public const string RegTypeFentanyl = "Fentanyl";
    public const string RegTypeHeroin = "Heroin";
    public const string RegTypeOxy = "Oxycodon";
    public const string RegTypeTramadol = "Tramadol";
    public const string RegTypeStimulants = "Stimulats";


    private bool _showDialog = false;
    private string _notifyRegistrationType = "Heroin";

    private void CloseNotifyDialog()
    {
        _showDialog = false;
        _ = InvokeAsync(StateHasChanged);
    }

    private Action OnNotifyFentanylClick => OnNotifyButtonClicked(RegTypeFentanyl);
    private Action OnNotifyHeroinClick => OnNotifyButtonClicked(RegTypeHeroin);
    private Action OnNotifyOxyClick => OnNotifyButtonClicked(RegTypeOxy);
    private Action OnNotifyTramadolClick => OnNotifyButtonClicked(RegTypeTramadol);
    private Action OnNotifyStimulantsClick => OnNotifyButtonClicked(RegTypeStimulants);

    private Action OnNotifyButtonClicked(string regType) => () =>
    {
        _notifyRegistrationType = regType;
        _showDialog = true;
        _ = InvokeAsync(StateHasChanged);
    };



}


