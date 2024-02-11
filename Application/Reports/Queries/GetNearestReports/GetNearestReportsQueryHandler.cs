using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using NetTopologySuite.Geometries;

namespace Application.Reports.Queries.GetNearestReports;

internal sealed class GetNearestReportsQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetNearestReportsQuery, Result<PagedList<GetCitizenReportsResponse>>>
{

    public async Task<Result<PagedList<GetCitizenReportsResponse>>> Handle(
        GetNearestReportsQuery request,
        CancellationToken cancellationToken)
    {
        var currentLocation = new Point(request.Longitude, request.Latitude) { SRID = 4326 };

        var context = unitOfWork.DbContext.Set<Report>();
        var query = context.Where(r => r.ReportState != ReportState.NeedAcceptance
                                       && r.Visibility == Visibility.EveryOne
                                       && r.ShahrbinInstanceId == request.InstanceId
                                       && r.Address.Location != null);
        var query2 = query
            .OrderBy(r => r.Address.Location!.Distance(currentLocation))
            .Select(r => GetCitizenReportsResponse.FromReport(r, request.UserId));

        var reports = await PagedList<GetCitizenReportsResponse>.ToPagedList(
           query2,
           request.PagingInfo.PageNumber,
           request.PagingInfo.PageSize);

        return reports;
    }
}
