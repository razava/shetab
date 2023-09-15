using Api.Configurations;
using Microsoft.Extensions.Options;

namespace Api.Services.Sms.Panels;

public class MeliPayamakSms : ISmsMessaging
{
    private readonly SmsOptions _smsOptions;

    public MeliPayamakSms(IOptions<SmsOptions> smsOptions)
    {
        _smsOptions = smsOptions.Value;
    }

    public async Task<int> SendAsync(string receptor, string message)
    {
        mpNuget.RestClient restClient = new mpNuget.RestClient(_smsOptions.MeliPayamakUsername, _smsOptions.MeliPayamakPassword);
        restClient.Send(receptor, _smsOptions.MeliPayamakPhoneNumber, message, true);

        await Task.CompletedTask;
        return 0;
    }

    public async Task<int> SendVerificationAsync(string receptor, string message)
    {
        mpNuget.RestClient restClient = new mpNuget.RestClient(_smsOptions.MeliPayamakUsername, _smsOptions.MeliPayamakPassword);
        restClient.SendByBaseNumber(message, receptor, int.Parse(_smsOptions.MeliPayamakPatternId));

        await Task.CompletedTask;
        return 0;
    }
}
