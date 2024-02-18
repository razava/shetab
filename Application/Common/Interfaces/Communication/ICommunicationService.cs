
using Domain.Models.Relational.ReportAggregate;

namespace Application.Common.Interfaces.Communication;

public interface ICommunicationService
{
    public void SendNotification(Message message);
    public Task<int> SendAsync(string receptor, string message);
    public Task<int> SendVerificationAsync(string receptor, string message);
    public Task AddCommunicationId(string userId, string communicationId);
    public Task SendNotification(string userId, string method, string message, Guid id);
    public Task SendNotification(List<string> userIds, string method, string message, Guid id);
}
