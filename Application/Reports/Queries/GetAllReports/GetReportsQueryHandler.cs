using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetReports;

internal sealed class GetAllReportsQueryHandler : IRequestHandler<GetAllReportsQuery, PagedList<Report>>
{
    private readonly IReportRepository _reportRepository;

    public GetAllReportsQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<PagedList<Report>> Handle(GetAllReportsQuery request, CancellationToken cancellationToken)
    {
        var reports = await _reportRepository.GetPagedAsync(
            request.PagingInfo,
            null,
            false,
            a => a.OrderBy(r => r.Sent));

        return reports;
    }
}
