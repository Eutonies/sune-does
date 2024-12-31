namespace SuneDoes.UI.Persistence.Model;

public class VerifiableEmailDbo
{

    public long EmailAddressId { get; set; }

    public string EmailAddress { get; set; }
    public string CodeString { get; set; }
    public DateTime? LastVerificationMailSent { get; set; }



}
