using SuneDoes.UI.Pages.Medicine;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuneDoes.UI.Persistence.Model;

public class MedicineNotificationDbo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long NotificationId { get; set; }
    public string Email { get; set; }
    public string NotifyType { get; set; }
    public string MedicineType { get; set; }
    public DateTime NotificationTime { get; set; }
    public string? Comment { get; set; }

    public MedicineNotification ToDomain() => new MedicineNotification(
        NotificationId: NotificationId,
        Email: Email,
        NotifyType: NotifyType,
        MedicineType: MedicineType,
        NotificationTime: NotificationTime,
        Comment: Comment
       );


}

public static class MedicineNotificationDboExtensions
{
    public static MedicineNotificationDbo ToDbo(this MedicineNotification notification) => new MedicineNotificationDbo
    {
        NotificationId = notification.NotificationId,
        Email = notification.Email.ToLower().Trim(),
        NotifyType = notification.NotifyType.ToLower().Trim(),
        MedicineType = notification.MedicineType,
        NotificationTime = notification.NotificationTime,
        Comment = notification.Comment
    };
}
