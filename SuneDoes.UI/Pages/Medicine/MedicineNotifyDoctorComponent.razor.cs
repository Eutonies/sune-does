using Microsoft.AspNetCore.Components;
using SuneDoes.Extensions;
using SuneDoes.UI.Components.Email;
using SuneDoes.UI.Components.Validation;

namespace SuneDoes.UI.Pages.Medicine;

public partial class MedicineNotifyDoctorComponent
{

    [Parameter]
    public string RegistrationType { get; set; }

    [Inject]
    public IVerifiableEmailHandler EmailHandler { get; set; }

    private VerifiableEmail? _verifiableEmail;
    private bool CanSendEmail => _verifiableEmail switch
    {
        null => false,
        VerifiableEmail em when em.Status == VerifiableEmailStatus.EmailVerified => false,
        VerifiableEmail em when em.Status == VerifiableEmailStatus.EmailSent && em.NextCanSendTime < DateTime.Now => true,
        VerifiableEmail em when em.Status == VerifiableEmailStatus.EmailSent => false,
        VerifiableEmail em when em.Status == VerifiableEmailStatus.InvalidEmail => false,
        _ => true
    };

    private async Task<(ValidationState Status, string? InfoMessage)> OnEmailUpdate(string? emailString)
    {
        if(string.IsNullOrWhiteSpace(emailString))
        {
            _verifiableEmail = null;
            _ = InvokeAsync(StateHasChanged);
            return (ValidationState.NotEvaluated, null);
        }
        if(!EmailHandler.IsValidEmailAddress(emailString))
        {
            _verifiableEmail = null;
            _ = InvokeAsync(StateHasChanged);
            return (ValidationState.Invalid, "Invalid email");
        }
        _verifiableEmail = await EmailHandler.EntryFor(emailString);
        _ = InvokeAsync(StateHasChanged);
        if(_verifiableEmail == null)
            return (ValidationState.NotEvaluated, null);
        if (_verifiableEmail.Status == VerifiableEmailStatus.EmailVerified)
            return (ValidationState.Valid, "Email verified");
        if (_verifiableEmail.Status == VerifiableEmailStatus.EmailSent)
            return (ValidationState.Info, "Verification email sent");
        if (_verifiableEmail.Status == VerifiableEmailStatus.InvalidEmail)
            return (ValidationState.Invalid, "Invalid email");
        return (ValidationState.NotEvaluated, null);

    }


    private string? _medicineType;
    private string? MedicineType
    {
        get => _medicineType;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _medicineType = value;
            else
                _medicineType = null;
        }
    }

    private string? _details;
    private string? Details
    {
        get => _details;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _details = value;
            else
                _details = null;
        }
    }

    protected override void OnParametersSet()
    {
        if (_medicineType == null)
        {
            _medicineType = RegistrationType;
            _ = InvokeAsync(StateHasChanged);
        }
    }

}
