
using Microsoft.AspNetCore.Components;
using SuneDoes.UI.Components;
using SuneDoes.UI.Components.Email;
using SuneDoes.UI.Session;

namespace SuneDoes.UI.Pages.Email;

public partial class VerifyEmailPage
{
    [SupplyParameterFromQuery(Name = ISuneDoesEmailSender.EmailIdQueryParameterName)]
    [Parameter]
    public string EmailId { get; set; }

    [SupplyParameterFromQuery(Name = ISuneDoesEmailSender.CodeStringQueryParameterName)]
    [Parameter]
    public string CodeString { get; set; }

    [Inject]
    public IVerifiableEmailHandler EmailHandler { get; set; }

    private string? _errorMessage;
    private string? _successMessage;

    protected override async Task OnParametersSetAsync()
    {
        if(_errorMessage == null && _successMessage == null)
        {
            try
            {
                if (!long.TryParse(EmailId, out var id))
                    _errorMessage = $"Email ID: {EmailId} is not valid";
                else
                {
                    var regRes = await EmailHandler.RegisterVerification(id, CodeString);
                    if (regRes != null)
                        _successMessage = $"Succesfully verified email address: {regRes.EmailAddress}";
                    else
                        _errorMessage = "I don't know what but SOMETHING went haywire!";
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            _ = InvokeAsync(StateHasChanged);
        }


    }


}
