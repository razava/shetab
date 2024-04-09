using Application.Common.Interfaces.Persistence;

namespace Application.Reports.Queries.GetPossibleTransitions;

internal sealed class GetPossibleTransitionsQueryHandler(
    IReportRepository reportRepository) 
    : IRequestHandler<GetPossibleTransitionsQuery, Result<List<PossibleTransitionResponse>>>
{
    public async Task<Result<List<PossibleTransitionResponse>>> Handle(GetPossibleTransitionsQuery request, CancellationToken cancellationToken)
    {
        var result = await reportRepository.GetPossibleTransition(request.reportId, request.userId);
        return result;
    }
}
