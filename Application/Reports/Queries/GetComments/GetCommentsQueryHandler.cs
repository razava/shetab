using Application.Common.Interfaces.Persistence;

namespace Application.Reports.Queries.GetComments;

internal sealed class GetCommentsQueryHandler(IReportRepository reportRepository) 
    : IRequestHandler<GetCommentsQuery, Result<PagedList<GetReportCommentsResponse>>>
{
    public async Task<Result<PagedList<GetReportCommentsResponse>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
       var result = await reportRepository.GetReportComments(
            request.ReportId,
            GetReportCommentsResponse.GetSelector(request.UserId),
            request.PagingInfo);

        return result;
    }
}