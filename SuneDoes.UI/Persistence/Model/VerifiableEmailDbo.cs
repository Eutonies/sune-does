using SuneDoes.UI.Components.Email;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuneDoes.UI.Persistence.Model;

public class VerifiableEmailDbo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long EmailAddressId { get; set; }

    public string EmailAddress { get; set; }
    public string CodeString { get; set; }
    public DateTime? LastVerificationMailSent { get; set; }

    public DateTime? VerifiedAt { get; set; }


    public VerifiableEmail ToDomain() => new VerifiableEmail(
        EmailAddressId: EmailAddressId,
        EmailAddress: EmailAddress,
        CodeString: CodeString,
        LastVerificationEmailSent: LastVerificationMailSent,
        VerifiedAt: VerifiedAt
        );



}


internal static class VerifiableEmailDboExtensions
{
    public static VerifiableEmailDbo ToDbo(this VerifiableEmail em) => new VerifiableEmailDbo
    {
        EmailAddressId = em.EmailAddressId,
        EmailAddress = em.EmailAddress.Trim().ToLower(),
        CodeString = em.CodeString,
        LastVerificationMailSent = em.LastVerificationEmailSent,
        VerifiedAt = em.VerifiedAt

    };
}