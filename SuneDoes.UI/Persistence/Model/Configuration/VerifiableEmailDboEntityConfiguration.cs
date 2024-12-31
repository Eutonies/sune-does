using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SuneDoes.UI.Persistence.Model.Configuration;

internal class VerifiableEmailDboEntityConfiguration : IEntityTypeConfiguration<VerifiableEmailDbo>
{

    public void Configure(EntityTypeBuilder<VerifiableEmailDbo> builder)
    {
        builder.ToTable("email_address");
        builder.Property(nameof(VerifiableEmailDbo.EmailAddressId)).HasColumnName("email_address_id");
        builder.Property(nameof(VerifiableEmailDbo.EmailAddress)).HasColumnName("email_address_string");
        builder.Property(nameof(VerifiableEmailDbo.CodeString)).HasColumnName("code_string");
        builder.Property(nameof(VerifiableEmailDbo.LastVerificationMailSent)).HasColumnName("last_verification_mail_sent");
        builder.Property(nameof(VerifiableEmailDbo.VerifiedAt)).HasColumnName("verified_at");
    }
}
