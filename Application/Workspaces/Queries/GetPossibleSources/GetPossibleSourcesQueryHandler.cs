using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Workspaces.Queries.GetPossibleSources;

public sealed class GetPossibleSourcesQueryHandler : IRequestHandler<GetPossibleSourcesQuery, List<PossibleSourceResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPossibleSourcesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<PossibleSourceResponse>> Handle(GetPossibleSourcesQuery request, CancellationToken cancellationToken)
    {
        var roleIds = await _unitOfWork.DbContext.Set<ApplicationRole>()
            .Where(r => r.Name != null && request.RoleNames.Contains(r.Name))
            .Select(r => r.Id)
            .ToListAsync();

        var stagesIds = await _unitOfWork.DbContext.Set<ProcessStage>()
            .Where(ps => ps.Actors.Any(
                a => a.Type == ActorType.Person && a.Identifier == request.UserId || 
                     a.Type == ActorType.Role && roleIds.Contains(a.Identifier)))
            .Select(ps => ps.Id)
            .ToListAsync();

        var possibleSources = await _unitOfWork.DbContext.Set<ProcessTransition>()
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
