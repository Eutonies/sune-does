namespace SuneDoes.UI.Components.Email;

public interface ISuneDoesEmailSender
{

    Task SendVerificationEmail(VerifiableEmail mail);

}
