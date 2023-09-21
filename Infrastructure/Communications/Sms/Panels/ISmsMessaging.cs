namespace Infrastructure.Communications.Sms.Panels;

public interface ISmsMessaging
{
    public Task<int> SendAsync(string receptor, string message);
    public Task<int> SendVerificationAsync(string receptor, string message);
}
