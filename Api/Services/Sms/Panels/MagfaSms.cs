using Api.Configurations;
using Microsoft.Extensions.Options;

namespace Api.Services.Sms.Panels;

public class MagfaSms : ISmsMessaging
{
    private readonly SmsOptions _smsOptions;

    public MagfaSms(IOptions<SmsOptions> smsOptions)
    {
        _smsOptions = smsOptions.Value;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        var client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = false
        });

        var url = $"{_smsOptions.MagfaUrl}?" +
            $"service=enqueue&" +
            $"username={_smsOptions.MagfaUsername}&" +
            $"password={_smsOptions.MagfaPassword}&" +
            $"domain={_smsOptions.MagfaDomain}&" +
            $"from={_smsOptions.MagfaPhoneNumber}&" +
            $"to={receptor}&" +
            $"text={message}";

        await client.GetAsync(url);

        return 0;
    }

    public async Task<int> SendVerificationAsync(string receptor, string message)
    {
        message = "سامانه شهربین\r\nکدورود:\r\n" + message;
        var client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = false
        });

        var url = $"{_smsOptions.MagfaUrl}?" +
            $"service=enqueue&" +
            $"username={_smsOptions.MagfaUsername}&" +
            $"password={_smsOptions.MagfaPassword}&" +
            $"domain={_smsOptions.MagfaDomain}&" +
            $"from={_smsOptions.MagfaPhoneNumber}&" +
            $"to={receptor}&" +
            $"text={message}";

        await client.GetAsync(url);

        return 0;
    }
}
