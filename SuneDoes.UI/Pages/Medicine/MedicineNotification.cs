namespace SuneDoes.UI.Pages.Medicine;

public record MedicineNotification(
    long NotificationId,
    string Email,
    string NotifyType,
    string MedicineType,
    DateTime NotificationTime,
    string? Comment
    );
