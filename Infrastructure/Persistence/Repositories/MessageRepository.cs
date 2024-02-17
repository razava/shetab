using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;

namespace Infrastructure.Persistence.Repositories;

public class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    public MessageRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
