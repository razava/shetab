namespace Application.Common.Interfaces.Communication;

public interface ISmsService
{
    public Task<int> SendAsync(string receptor, string message);
    public Task<int> SendVerificationAsync(string receptor, string message);
}
