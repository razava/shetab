using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetUserReports;

internal sealed class GetUserReportsQueryHandler : IRequestHandler<GetUserReportsQuery, PagedList<Report>>
{
    private readonly IReportRepository _reportRepository;

    public GetUserReportsQueryHandler(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<PagedList<Report>> Handle(GetUserReportsQuery request, CancellationToken cancellationToken)
    {
        var reports = await _reportRepository.GetPagedAsync(
            request.PagingInfo,
            r => r.CitizenId == request.UserId,
            false,
            o => o.OrderByDescending(r => r.LastStatusDateTime));

        return reports;
    }
}
