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

    [Parameter]
    public int YOffSet { get; set; }

    [Inject]
    public IVerifiableEmailHandler EmailHandler { get; set; }
    private bool _hasRegisteredEmailListener = false;

    [Inject]
    public IMedicineNotificationHandler NotificationHandler { get; set; }


    private void OnEmailVerification(object? sender, VerifiableEmail email)
    {
        if(_currentEmailString != null && email.EmailAddress.ToLower().Trim() == _currentEmailString.ToLower().Trim())
        {
            _verifiableEmail = email;
            _ = OnEmailUpdate(_verifiableEmail?.EmailAddress);
            _ = InvokeAsync(StateHasChanged);
        }
    }


    private MedicineNotification? _existingNotification;
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
    private string? _currentEmailValidationInfo;
    private ValidationState _currentEmailValidationState = ValidationState.NotEvaluated;
    private async Task<(ValidationState Status, string? InfoMessage)> OnEmailUpdate(string? emailString)
    {
        _currentEmailString = emailString;
        _existingNotification = null;
        if(string.IsNullOrWhiteSpace(_currentEmailString))
        {
            _verifiableEmail = null;
            (_currentEmailValidationState, _currentEmailValidationInfo) = (ValidationState.NotEvaluated, null);
        }
        else if(!EmailHandler.IsValidEmailAddress(_currentEmailString))
        {
            _verifiableEmail = null;
            (_currentEmailValidationState, _currentEmailValidationInfo) = (ValidationState.Invalid, "Invalid email");
        }
        else
        {
            _verifiableEmail = await EmailHandler.LoadEntryFor(_currentEmailString);
            if (_verifiableEmail == null)
                (_currentEmailValidationState, _currentEmailValidationInfo) = (ValidationState.Info, "Email address not verified");
            else if (_verifiableEmail.Status == VerifiableEmailStatus.EmailVerified)
            {
                (_currentEmailValidationState, _currentEmailValidationInfo) = (ValidationState.Valid, "Email verified");
                _existingNotification = await NotificationHandler.ExistingNotificationFor(_currentEmailString, RegistrationType);
            }
            else if (_verifiableEmail.Status == VerifiableEmailStatus.EmailSent)
                (_currentEmailValidationState, _currentEmailValidationInfo) = (ValidationState.Info, "Verification email sent");
            else if (_verifiableEmail.Status == VerifiableEmailStatus.InvalidEmail)
                (_currentEmailValidationState, _currentEmailValidationInfo) = (ValidationState.Invalid, "Invalid email");
            else
                (_currentEmailValidationState, _currentEmailValidationInfo) = (ValidationState.NotEvaluated, null);
        }
        EvaluateCorrectness();
        _ = InvokeAsync(StateHasChanged);
        return (_currentEmailValidationState, _currentEmailValidationInfo);

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
            EvaluateCorrectness();
            _ = InvokeAsync(StateHasChanged);
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
            EvaluateCorrectness();
            _ = InvokeAsync(StateHasChanged);
        }
    }

    protected override void OnParametersSet()
    {
        if (_medicineType == null)
        {
            _medicineType = RegistrationType;
            EvaluateCorrectness();
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
                _ = OnEmailUpdate(_verifiableEmail?.EmailAddress);
            });
        }
    }


    private List<string> _errorMessages = new List<string>();

    private void EvaluateCorrectness()
    {
        _errorMessages.Clear();
        if (_verifiableEmail == null || _verifiableEmail.Status == VerifiableEmailStatus.InvalidEmail)
            _errorMessages.Add("Please specify a valid email address where the doctor can reach out to you");
        else if(_verifiableEmail.Status == VerifiableEmailStatus.EmailSent)
            _errorMessages.Add("Please verify email address (you may have to sift through your spam filter for verification mail)");
        else if (_verifiableEmail.Status == VerifiableEmailStatus.Registered || _verifiableEmail.Status == VerifiableEmailStatus.NotRegistered)
            _errorMessages.Add("Please verify email address");
        else if(_existingNotification != null)
        {
            _errorMessages.Add($"The doctor has already received notification from {_currentEmailString} for this medicine type on {_existingNotification.NotificationTime.ToString("dd-MM-yyyy HH:mm:ss")}");
        }

        if(string.IsNullOrWhiteSpace(_medicineType))
            _errorMessages.Add("Please specify what type of medicine your request pertains to");

        if (string.IsNullOrWhiteSpace(_details) || _details.Length < 5)
            _errorMessages.Add("Please provide a short description of your situation");

    }

    private void OnCancelClicked()
    {
        CloseDialog();
    }

    private async void OnSubmitClicked()
    {
        if(_verifiableEmail != null && !string.IsNullOrWhiteSpace(_medicineType) && !string.IsNullOrWhiteSpace(RegistrationType) && !string.IsNullOrWhiteSpace(_details))
        {
            var notificaion = new MedicineNotification(
                NotificationId: 0L,
                Email: _verifiableEmail.EmailAddress,
                NotifyType: RegistrationType,
                MedicineType: _medicineType,
                NotificationTime: DateTime.Now,
                Comment: _details
                );
            try
            {
                await NotificationHandler.SubmitNotification(notificaion);
                CloseDialog();
            }
            catch (Exception ex)
            {

            }

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
