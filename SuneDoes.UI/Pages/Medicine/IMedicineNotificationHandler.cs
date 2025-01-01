namespace SuneDoes.UI.Pages.Medicine;

public interface IMedicineNotificationHandler
{
    Task SubmitNotification(MedicineNotification notification);
    Task<MedicineNotification?> ExistingNotificationFor(string email, string notifyType);


}
