
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
    private int _dialogOffset = 0;
    private string _notifyRegistrationType = "Heroin";

    private void CloseNotifyDialog()
    {
        _showDialog = false;
        _ = InvokeAsync(StateHasChanged);
    }

    private void OnNotifyFentanylClick(double yCoordinate)  => OnNotifyButtonClicked(RegTypeFentanyl, yCoordinate);
    private void OnNotifyHeroinClick(double yCoordinate) => OnNotifyButtonClicked(RegTypeHeroin, yCoordinate);
    private void OnNotifyOxyClick(double yCoordinate) => OnNotifyButtonClicked(RegTypeOxy, yCoordinate);
    private void OnNotifyTramadolClick(double yCoordinate) => OnNotifyButtonClicked(RegTypeTramadol, yCoordinate);
    private void OnNotifyStimulantsClick(double yCoordinate) => OnNotifyButtonClicked(RegTypeStimulants, yCoordinate);

    private void OnNotifyButtonClicked(string regType, double yCoordinate)
    {
        _notifyRegistrationType = regType;
        _showDialog = true;
        _dialogOffset = (int) yCoordinate;
        _ = InvokeAsync(StateHasChanged);
    }



}


