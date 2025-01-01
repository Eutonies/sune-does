using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SuneDoes.UI.Persistence.Model.Configuration;

internal class MedicineNotificationDboEntityConfiguration : IEntityTypeConfiguration<MedicineNotificationDbo>
{

    public void Configure(EntityTypeBuilder<MedicineNotificationDbo> builder)
    {
        builder.ToTable("medicine_notification");
        builder.Property(nameof(MedicineNotificationDbo.NotificationId)).HasColumnName("notification_id");
        builder.Property(nameof(MedicineNotificationDbo.Email)).HasColumnName("email_address");
        builder.Property(nameof(MedicineNotificationDbo.NotifyType)).HasColumnName("notify_type");
        builder.Property(nameof(MedicineNotificationDbo.MedicineType)).HasColumnName("medicine_type");
        builder.Property(nameof(MedicineNotificationDbo.NotificationTime)).HasColumnName("notification_time");
        builder.Property(nameof(MedicineNotificationDbo.Comment)).HasColumnName("notification_comment");
    }
}
