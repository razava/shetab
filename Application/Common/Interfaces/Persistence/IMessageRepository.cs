using Domain.Models.Relational.ReportAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IMessageRepository : IGenericRepository<Message>
{
    Task<List<MessageWithReciepient>> GetToSendSms(int count);
    Task SetAsSend(List<Guid> ids);
}

public record MessageWithReciepient(Guid MessageId, string PhoneNumber, string Content);