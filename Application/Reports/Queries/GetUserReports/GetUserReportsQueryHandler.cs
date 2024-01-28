using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetUserReports;

internal sealed class GetUserReportsQueryHandler(IReportRepository reportRepository) : IRequestHandler<GetUserReportsQuery, Result<PagedList<Report>>>
{
    public async Task<Result<PagedList<Report>>> Handle(GetUserReportsQuery request, CancellationToken cancellationToken)
    {
        var reports = await reportRepository.GetPagedAsync(
            request.PagingInfo,
            r => r.CitizenId == request.UserId,
            false,
            o => o.OrderByDescending(r => r.LastStatusDateTime));

        return reports;
    }
}
