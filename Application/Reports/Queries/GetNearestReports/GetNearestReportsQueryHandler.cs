using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;
using NetTopologySuite.Geometries;

namespace Application.Reports.Queries.GetNearestReports;

internal sealed class GetNearestReportsQueryHandler(IReportRepository reportRepository) : IRequestHandler<GetNearestReportsQuery, Result<PagedList<Report>>>
{

    public async Task<Result<PagedList<Report>>> Handle(GetNearestReportsQuery request, CancellationToken cancellationToken)
    {
        var currentLocation = new Point(request.Longitude, request.Latitude) { SRID = 4326 };
        var reports = await reportRepository.GetPagedAsync(
            request.PagingInfo,
            r => r.ReportState != ReportState.NeedAcceptance
                 && r.Visibility == Visibility.EveryOne
                 && r.ShahrbinInstanceId == request.InstanceId
                 && r.Address.Location != null,
            false,
            o => o.OrderBy(r => r.Address.Location!.Distance(currentLocation)));

        return reports;
    }
}
