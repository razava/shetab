using Application.Common.Interfaces.Persistence;


namespace Application.Reports.Queries.GetReportById;

internal sealed class GetHistoryQueryHandler(
    IReportRepository reportRepository) : IRequestHandler<GetHistoryQuery, Result<List<HistoryResponse>>>
{
    public async Task<Result<List<HistoryResponse>>> Handle(GetHistoryQuery request, CancellationToken cancellationToken)
    {
        //TODO: check whether user can access to content or not
        var result = await reportRepository.GetReportHistory(request.Id);
        return result;
    }
}