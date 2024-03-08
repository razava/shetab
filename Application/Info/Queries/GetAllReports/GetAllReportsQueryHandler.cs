using Application.Common.Interfaces.Info;
using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Info.Queries.GetAllReports;

internal sealed class GetAllReportsQueryHandler(IInfoService infoService)
    : IRequestHandler<GetAllReportsQuery, Result<PagedList<GetReportsResponse>>>
{

    public async Task<Result<PagedList<GetReportsResponse>>> Handle(
        GetAllReportsQuery request, CancellationToken cancellationToken)
    {
        var queryParameters = new GetInfoQueryParameters(
            request.InstanceId,
            request.UserId,
            request.Roles,
            null,
            request.ReportFilters,
            request.ReportsToInclude,
            request.Geometry);


        var result = await infoService.GetReports(queryParameters, GetReportsResponse.GetSelector(), request.PagingInfo);


        return result;
    }
}
