using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ProcessAggregate;

namespace Infrastructure.Persistence.Repositories;

public class ActorRepository: GenericRepository<Actor>, IActorRepository
{
    public ActorRepository(
        ApplicationDbContext dbContext) : base(dbContext)
    {       
    }
}
