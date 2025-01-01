namespace SuneDoes.UI.Components.Email;

public interface IVerifiableEmailHandler
{

    event EventHandler<VerifiableEmail> OnUpdate;
    bool IsValidEmailAddress(string email);
    Task<VerifiableEmail?> LoadEntryFor(string email);
    Task<VerifiableEmail?> EnsureEntryFor(string email);
    Task<VerifiableEmail> RegisterVerification(long emailId, string codeString);

    Task<VerifiableEmail?> SendVerificationMail(string email);

}
