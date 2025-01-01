
using Microsoft.EntityFrameworkCore;
using SuneDoes.UI.Persistence.Context;
using SuneDoes.UI.Persistence.Model;
using System.Net.Mail;

namespace SuneDoes.UI.Components.Email;

public class VerifiableEmailHandler : IVerifiableEmailHandler
{

    private readonly IDbContextFactory<SuneDoesDbContext> _contextFactory;
    private readonly SemaphoreSlim _updateLock = new SemaphoreSlim(1);
    private readonly IServiceScopeFactory _scopeFactory;

    public VerifiableEmailHandler(IDbContextFactory<SuneDoesDbContext> contextFactory, IServiceScopeFactory scopeFactory)
    {
        _contextFactory = contextFactory;
        _scopeFactory = scopeFactory;
    }

    public event EventHandler<VerifiableEmail> OnUpdate;

    public Task<VerifiableEmail?> EnsureEntryFor(string email) => WithContext(async cont =>
    {
        if (!IsValidEmailAddress(email))
            return null;
        email = email
            .ToLower()
            .Trim();
        var returnee = await Locked(async () =>
        {
            var existing = await cont.EmailAddresses
                .FirstOrDefaultAsync(_ => _.EmailAddress == email);
            if (existing != null)
                return existing.ToDomain();
            var insertee = new VerifiableEmailDbo
            {
                EmailAddress = email,
                CodeString = Guid.NewGuid()
                  .ToString()
                  .Replace("-", "")
            };
            cont.Add(insertee);
            await cont.SaveChangesAsync();
            return insertee.ToDomain();
        });
        return returnee;
    });

    public bool IsValidEmailAddress(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        email = email
            .ToLower()
            .Trim();
        var isValid = true;
        try
        {
            var addr = new MailAddress(email);
            isValid = (addr.Address == email);
        }
        catch 
        {
            isValid = false;
        }
        return isValid;
    }

    public Task<VerifiableEmail> RegisterVerification(long emailId, string codeString) => WithContext(async cont =>
    {
        var returnee = await Locked(async () => 
        {
            var loaded = await cont.EmailAddresses
               .FirstOrDefaultAsync(_ => _.EmailAddressId == emailId && _.CodeString == codeString);
            if (loaded == null)
                throw new Exception($"Did not find a verifiable email with ID: {emailId} and code string: {codeString}");
            if (loaded.VerifiedAt != null)
                return loaded.ToDomain();
            loaded.VerifiedAt = DateTime.Now;
            cont.Update(loaded);
            await cont.SaveChangesAsync();
            return loaded.ToDomain();
        });
        OnUpdate?.Invoke(this, returnee);
        return returnee;
    });


    private async Task<T> WithContext<T>(Func<SuneDoesDbContext,Task<T>> toPerform)
    {
        await using var cont = await _contextFactory.CreateDbContextAsync();
        var returnee = await toPerform(cont);
        return returnee;
    }

    private async Task<T> Locked<T>(Func<Task<T>> toPerform)
    {
        await _updateLock.WaitAsync();
        try
        {
            return await toPerform();
        }
        finally
        {
            _updateLock.Release();
        }
    }

    public Task<VerifiableEmail?> SendVerificationMail(string email) => WithContext(async cont =>
    {
        var entry = await EnsureEntryFor(email);
        if (entry == null)
            return null;
        var returnee = await Locked(async () => 
        {
            using var scope = _scopeFactory.CreateScope();
            var emailSender = scope.ServiceProvider.GetRequiredService<ISuneDoesEmailSender>();
            await emailSender.SendVerificationEmail(entry);
            var updatee = entry.ToDbo();
            updatee.LastVerificationMailSent = DateTime.Now;
            cont.Update(updatee);
            await cont.SaveChangesAsync();
            return updatee.ToDomain();
        });
        return returnee;  
    });

    public Task<VerifiableEmail?> LoadEntryFor(string email) => WithContext(async cont =>
    {
        if(!IsValidEmailAddress(email)) return null;
        email = email.ToLower().Trim();
        var loaded = await cont.EmailAddresses
            .FirstOrDefaultAsync(_ => _.EmailAddress == email);
        return loaded?.ToDomain();

    });
}
