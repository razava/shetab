using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Queries.GetNearestReports;

internal sealed class GetNearestReportsQueryHandler : IRequestHandler<GetNearestReportsQuery, PagedList<Report>>
{
    private readonly IReportRepository _reportRepository;

    public GetNearestReportsQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<PagedList<Report>> Handle(GetNearestReportsQuery request, CancellationToken cancellationToken)
    {
        var reports = await _reportRepository.GetPagedAsync(
            request.PagingInfo,
            r => r.Visibility == Visibility.EveryOne,
            false,
            o => o.OrderByDescending(r => r.LastStatusDateTime));

        return reports;
    }
}
