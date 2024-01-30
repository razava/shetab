using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Workspaces.Queries.GetPossibleSources;

public sealed class GetPossibleSourcesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPossibleSourcesQuery, Result<List<PossibleSourceResponse>>>
{
    public async Task<Result<List<PossibleSourceResponse>>> Handle(GetPossibleSourcesQuery request, CancellationToken cancellationToken)
    {
        var roleIds = await unitOfWork.DbContext.Set<ApplicationRole>()
            .Where(r => r.Name != null && request.RoleNames.Contains(r.Name))
            .Select(r => r.Id)
            .ToListAsync();

        var stagesIds = await unitOfWork.DbContext.Set<ProcessStage>()
            .Where(ps => ps.Actors.Any(
                a => a.Type == ActorType.Person && a.Identifier == request.UserId || 
                     a.Type == ActorType.Role && roleIds.Contains(a.Identifier)))
            .Select(ps => ps.Id)
            .ToListAsync();

        var possibleSources = await unitOfWork.DbContext.Set<ProcessTransition>()
            .Where(t => stagesIds.Contains(t.ToId))
            .Select(t => t.From.DisplayRole)
            .GroupBy(dr => dr.Id)
            .Select(p => new PossibleSourceResponse(
                p.Key,
                p.Select(q => q.Name).FirstOrDefault() ?? "",
                p.Select(q => q.Title).FirstOrDefault() ?? ""))
            .ToListAsync();

        return possibleSources;
    }
}
