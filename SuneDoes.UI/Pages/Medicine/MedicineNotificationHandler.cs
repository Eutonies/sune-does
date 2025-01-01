
using Microsoft.EntityFrameworkCore;
using SuneDoes.UI.Components.Email;
using SuneDoes.UI.Persistence.Context;
using SuneDoes.UI.Persistence.Model;

namespace SuneDoes.UI.Pages.Medicine;

public class MedicineNotificationHandler : IMedicineNotificationHandler
{
    private readonly SemaphoreSlim _updateLock = new SemaphoreSlim(1);
    private readonly IDbContextFactory<SuneDoesDbContext> _contextFactory;
    private readonly IServiceScopeFactory _scopeFactory;

    public MedicineNotificationHandler(IDbContextFactory<SuneDoesDbContext> contextFactory, IServiceScopeFactory scopeFactory)
    {
        _contextFactory = contextFactory;
        _scopeFactory = scopeFactory;
    }

    public async Task<MedicineNotification?> ExistingNotificationFor(string email, string notifyType)
    {
        await using var cont = await _contextFactory.CreateDbContextAsync();
        var returnee = await Load(email, notifyType, cont);
        return returnee;
    }

    public async Task SubmitNotification(MedicineNotification notification)
    {
        await _updateLock.WaitAsync();
        try
        {
            await using var cont = await _contextFactory.CreateDbContextAsync();
            var existing = await Load(notification.Email, notification.NotifyType, cont);
            if (existing != null)
                throw new Exception($"Existing notification for: {notification.Email} regarding {notification.NotifyType} already exists");
            var insertee = notification.ToDbo();
            cont.Add(insertee);
            await cont.SaveChangesAsync();
            using var scope = _scopeFactory.CreateScope();
            var emailSender = scope.ServiceProvider.GetRequiredService<ISuneDoesEmailSender>();
            await emailSender.SendNotificationEmail(notification);
        }
        finally
        {
            _updateLock.Release();
        }
    }

    private async Task<MedicineNotification?> Load(string email, string notifyType, SuneDoesDbContext cont)
    {
        if (email == null || notifyType == null)
            return null;
        email = email.Trim().ToLower();
        notifyType = notifyType.Trim().ToLower();
        var existing = await cont.Notifications
            .FirstOrDefaultAsync(_ => _.Email == email && _.NotifyType == notifyType);
        var returnee = existing?.ToDomain();
        return returnee;
    }



}
