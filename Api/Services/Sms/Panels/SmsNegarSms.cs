using Api.Configurations;
using Microsoft.Extensions.Options;

namespace Api.Services.Sms.Panels;

public class SmsNegarSms : ISmsMessaging
{
    private readonly SmsOptions _smsOptions;

    public SmsNegarSms(IOptions<SmsOptions> smsOptions)
    {
        _smsOptions = smsOptions.Value;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        var client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = false
        });

        var url = $"{_smsOptions.SmsNegarUrl}?" +
            $"cbody={message}&cmobileno={receptor}&" +
            $"cUsername={_smsOptions.SmsNegarUsername}&cpassword={_smsOptions.SmsNegarPassword}&" +
            $"cDomainName={_smsOptions.SmsNegarDomain}&cEncoding=1&cfromnumber={_smsOptions.SmsNegarPhoneNumber}";

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

        var url = $"{_smsOptions.SmsNegarUrl}?" +
            $"cbody={message}&cmobileno={receptor}&" +
            $"cUsername={_smsOptions.SmsNegarUsername}&cpassword={_smsOptions.SmsNegarPassword}&" +
            $"cDomainName={_smsOptions.SmsNegarDomain}&cEncoding=1&cfromnumber={_smsOptions.SmsNegarPhoneNumber}";

        await client.GetAsync(url);

        return 0;
    }
}
