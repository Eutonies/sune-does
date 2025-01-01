namespace SuneDoes.UI.Components.Email;

public interface ISuneDoesEmailSender
{
    public const string EmailIdQueryParameterName = "emailid";
    public const string CodeStringQueryParameterName = "codestring";

    Task SendVerificationEmail(VerifiableEmail mail);

}
