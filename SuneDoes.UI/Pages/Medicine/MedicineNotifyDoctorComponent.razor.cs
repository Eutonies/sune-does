using Microsoft.AspNetCore.Components;
using SuneDoes.Extensions;
using SuneDoes.UI.Components.Email;
using SuneDoes.UI.Components.Validation;

namespace SuneDoes.UI.Pages.Medicine;

public partial class MedicineNotifyDoctorComponent : IDisposable
{

    [Parameter]
    public string RegistrationType { get; set; }

    [Parameter]
    public Action CloseDialog { get; set; }

    [Inject]
    public IVerifiableEmailHandler EmailHandler { get; set; }
    private bool _hasRegisteredEmailListener = false;


    private void OnEmailVerification(object? sender, VerifiableEmail email)
    {
        if(_currentEmailString != null && email.EmailAddress.ToLower().Trim() == _currentEmailString.ToLower().Trim())
        {
            _verifiableEmail = email;
            _ = InvokeAsync(StateHasChanged);
        }
    }



    private VerifiableEmail? _verifiableEmail;
    private bool CanSendEmail => _verifiableEmail switch
    {
        null when _currentEmailString != null && EmailHandler.IsValidEmailAddress(_currentEmailString) => true,
        null => false,
        VerifiableEmail em when em.Status == VerifiableEmailStatus.EmailVerified => false,
        VerifiableEmail em when em.Status == VerifiableEmailStatus.EmailSent && em.NextCanSendTime < DateTime.Now => true,
        VerifiableEmail em when em.Status == VerifiableEmailStatus.EmailSent => false,
        VerifiableEmail em when em.Status == VerifiableEmailStatus.InvalidEmail => false,
        _ => true
    };
    private string? _currentEmailString;
    private async Task<(ValidationState Status, string? InfoMessage)> OnEmailUpdate(string? emailString)
    {
        _currentEmailString = emailString;
        if(string.IsNullOrWhiteSpace(_currentEmailString))
        {
            _verifiableEmail = null;
            _ = InvokeAsync(StateHasChanged);
            return (ValidationState.NotEvaluated, null);
        }
        if(!EmailHandler.IsValidEmailAddress(_currentEmailString))
        {
            _verifiableEmail = null;
            _ = InvokeAsync(StateHasChanged);
            return (ValidationState.Invalid, "Invalid email");
        }
        _verifiableEmail = await EmailHandler.LoadEntryFor(_currentEmailString);
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
        if(!_hasRegisteredEmailListener)
        {
            EmailHandler.OnUpdate += OnEmailVerification;
            _hasRegisteredEmailListener = true;
        }
    }

    private void OnSendClicked()
    {
        if(_currentEmailString != null && CanSendEmail)
        {
            _ = Task.Run(async () =>
            {
                _verifiableEmail = await EmailHandler.SendVerificationMail(_currentEmailString);
                _ = InvokeAsync(StateHasChanged);
            });
        }
    }

    public void Dispose()
    {
        if(_hasRegisteredEmailListener)
        {
            EmailHandler.OnUpdate -= OnEmailVerification;
            _hasRegisteredEmailListener = false;
        }
    }
}
