using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;
using NetTopologySuite.Geometries;

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
        var currentLocation = new Point(request.Longitude, request.Latitude);
        var reports = await _reportRepository.GetPagedAsync(
            request.PagingInfo,
            r => r.Visibility == Visibility.EveryOne && r.ShahrbinInstanceId == request.InstanceId && r.Address.Location != null,
            false,
            o => o.OrderBy(r => r.Address.Location!.Distance(currentLocation)));

        return reports;
    }
}
