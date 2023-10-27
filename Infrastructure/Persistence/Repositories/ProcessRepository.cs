using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.ProcessAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ProcessRepository : GenericRepository<Process>, IProcessRepository
{
    public ProcessRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<Process?> GetByIDAsync(int id, bool trackChanges = true)
    {
        var includedStage = @"Stages,Stages.Actors,Stages.Actors.Regions,Stages.Actors.BotActor,Stages.Actors.BotActor.Transition,Stages.Actors.BotActor.DestinationActors";
        var includedTransition = @"Transitions,Transitions.From,Transitions.To,Transitions.ReasonList";
        var includedRevisionUnit = @"RevisionUnit";
        var result = await base.GetSingleAsync(p => p.Id == id, trackChanges, $"{includedStage},{includedTransition},{includedRevisionUnit}");

        return result;
    }
}
