using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Queries.GetRecentReports;

internal sealed class GetRecentReportsQueryHandler : IRequestHandler<GetRecentReportsQuery, PagedList<Report>>
{
    private readonly IReportRepository _reportRepository;

    public GetRecentReportsQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<PagedList<Report>> Handle(GetRecentReportsQuery request, CancellationToken cancellationToken)
    {
        var reports = await _reportRepository.GetPagedAsync(
            request.PagingInfo,
            r => r.ReportState != ReportState.NeedAcceptance
                 && r.Visibility == Visibility.EveryOne
                 && r.ShahrbinInstanceId == request.instanceId,
            false,
            o => o.OrderByDescending(r => r.LastStatusDateTime));

        return reports;
    }
}
