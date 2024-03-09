using Application.Common.Interfaces.Info;

namespace Application.Info.Queries.GetExcel;

internal sealed class GetExcelQueryHandler(IInfoService infoService)
    : IRequestHandler<GetExcelQuery, Result<MemoryStream>>
{

    public async Task<Result<MemoryStream>> Handle(
        GetExcelQuery request, CancellationToken cancellationToken)
    {
        var queryParameters = new GetInfoQueryParameters(
            request.InstanceId,
            request.UserId,
            request.Roles,
            null,
            request.ReportFilters,
            request.ReportsToInclude,
            request.Geometry);


        var result = await infoService.GetExcel(queryParameters);


        return result;
    }
}
