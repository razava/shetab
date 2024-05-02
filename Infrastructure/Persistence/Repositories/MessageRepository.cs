using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    ApplicationDbContext context;
    public MessageRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        context = dbContext;
    }

    public async Task<int> GetMessageCount(string userId, DateTime from)
    {
        var result = await context.Message.AsNoTracking()
            .Where(m => m.Recepient.Type == RecepientType.Person
                && m.Recepient.ToId == userId
                && m.DateTime > from)
            .CountAsync();

        return result;
    }

    public async Task<PagedList<T>> GetMessages<T>(string userId, Expression<Func<Message, T>> selector, PagingInfo pagingInfo)
    {
        var query = context.Message.AsNoTracking()
            .Where(m => m.Recepient.Type == RecepientType.Person
                && m.Recepient.ToId == userId)
            .OrderByDescending(m => m.DateTime)
            .Select(selector);

        return await PagedList<T>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
    }

    public async Task<List<MessageWithReciepient>> GetToSendSms(int count)
    {
        var result = await context.Set<Message>()
            .Where(m => (
                m.MessageSendingType == MessageSendingType.Sms || 
                m.MessageSendingType == MessageSendingType.Both) 
                && m.LastSentSms == null)
            .OrderBy(m => m.DateTime)
            .Take(count)
            .Select(m => new MessageWithReciepient(
                m.Id, 
                context.Set<ApplicationUser>()
                    .Where(u => u.Id == m.Recepient.ToId)
                    .Select(u => u.PhoneNumber)
                    .FirstOrDefault() ?? "",
                $"سامانه شهربین \n {m.Title} \n {m.Content}"))
            .ToListAsync();

        return result;
    }

    public async Task SetAsSent(List<Guid> ids)
    {
        var now = DateTime.UtcNow;
        await context.Set<Message>()
            .Where(m => ids.Contains(m.Id))
            .ExecuteUpdateAsync(setters => setters.SetProperty(m => m.LastSentSms, now));
    }
}
