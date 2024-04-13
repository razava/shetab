using Application.Common.Interfaces.Persistence;

namespace Application.Workspaces.Queries.GetPossibleSources;

public sealed class GetPossibleSourcesQueryHandler(IReportRepository reportRepository) 
    : IRequestHandler<GetPossibleSourcesQuery, Result<List<PossibleSourceResponse>>>
{
    public async Task<Result<List<PossibleSourceResponse>>> Handle(GetPossibleSourcesQuery request, CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetPossibleSources(request.UserId, request.RoleNames);

        return result;
    }
}
