using Domain.Models.Relational.ReportAggregate;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IMessageRepository : IGenericRepository<Message>
{
    Task<List<MessageWithReciepient>> GetToSendSms(int count);
    Task SetAsSent(List<Guid> ids);
    Task<int> GetMessageCount(string userId, DateTime from);
    Task<PagedList<T>> GetMessages<T>(string userId, Expression<Func<Message, T>> selector, PagingInfo pagingInfo);
}

public record MessageWithReciepient(Guid MessageId, string PhoneNumber, string Content);