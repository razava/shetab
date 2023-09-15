using Api.Configurations;
using Microsoft.Extensions.Options;

namespace Api.Services.Sms.Panels;

public class IpSms : ISmsMessaging
{
    private readonly SmsOptions _smsOptions;

    public IpSms(IOptions<SmsOptions> smsOptions)
    {
        _smsOptions = smsOptions.Value;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("AccessKey", _smsOptions.IpPanelAccessKey);
        var smsApi = new IpPanelSmsApi.SmsApi(client);
        var response = await smsApi.SendSMSAsync(
            new IpPanelSmsApi.Body()
            {
                Message = message,
                Originator = _smsOptions.IpPanelPhoneNumber,
                Recipients = new List<string>() { receptor }
            });
        //TODO: Consider return values
        return 0;
    }

    public async Task<int> SendVerificationAsync(string receptor, string message)
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("AccessKey", _smsOptions.IpPanelAccessKey);
        var smsApi = new IpPanelSmsApi.SmsApi(client);
        var response = await smsApi.SendPatternAsync(new IpPanelSmsApi.Body3()
        {
            Originator = _smsOptions.IpPanelPhoneNumber,
            Pattern_code = _smsOptions.IpPanelPatternId,
            Recipient = receptor,
            Values = new { Code = message }
        });
        //TODO: Consider return values
        return 0;
    }
}
