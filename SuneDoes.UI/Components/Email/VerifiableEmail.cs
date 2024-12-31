namespace SuneDoes.UI.Components.Email;

public record VerifiableEmail(
    long EmailAddressId,
    string EmailAddress,
    string CodeString,
    DateTime? LastVerificattionEmailSent)
{



}
