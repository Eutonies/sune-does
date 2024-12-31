
using Microsoft.Extensions.Options;
using SuneDoes.UI.Configuration;

namespace SuneDoes.UI.Components.Email;

public class SuneDoesEmailSender : ISuneDoesEmailSender
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _emailApiBaseUrl;
    private readonly string _emailApiToken;

    public SuneDoesEmailSender(IHttpClientFactory httpClientFactory, IOptions<SuneDoesConfiguration> conf)
    {
        _httpClientFactory = httpClientFactory;
        _emailApiBaseUrl = conf.Value.Email.ApiBaseAddress;
        _emailApiToken = conf.Value.Email.ApiToken;
    }

    public async Task SendVerificationEmail(VerifiableEmail mail)
    {
        using var client = _httpClientFactory.CreateClient();
    }

    private record SendEmailLayout(
        SendPersonLayout from,
        SendPersonLayout[] to,
        SendPersonLayout[]? cc,
        SendPersonLayout[]? bcc,
        string? subject,
        string? html
        );

    private record SendPersonLayout(
        string email,
        string? name
        );


    private record SendResponseError(
        string? message
        
        
        );
}
