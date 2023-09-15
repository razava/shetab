using Api.Configurations;
using Microsoft.Extensions.Options;

namespace Api.Services.Sms.Panels;

public class KavehNegarSms : ISmsMessaging
{
    private readonly SmsOptions _smsOptions;

    public KavehNegarSms(IOptions<SmsOptions> smsOptions)
    {
        _smsOptions = smsOptions.Value;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        string apiKey = _smsOptions.ApiKey;
        string phoneNumber = _smsOptions.PhoneNumber;
        Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
        await api.Send(phoneNumber, receptor, message);

        return 0;
    }

    public async Task<int> SendVerificationAsync(string receptor, string message)
    {
        string apiKey = _smsOptions.ApiKey;
        Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
        await api.VerifyLookup(receptor, message, _smsOptions.TemplateName);

        return 0;
    }
}
