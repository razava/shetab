using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetAllReports;

internal sealed class GetAllReportsQueryHandler : IRequestHandler<GetAllReportsQuery, PagedList<Report>>
{
    private readonly IReportRepository _reportRepository;

    public GetAllReportsQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<PagedList<Report>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
    {
        //TODO: Implement appropriate filters
        var reports = await _reportRepository.GetPagedAsync(
            request.PagingInfo,
            r => r.ShahrbinInstanceId == request.instanceId,
            false,
            a => a.OrderBy(r => r.Sent));

        return reports;
    }
}
