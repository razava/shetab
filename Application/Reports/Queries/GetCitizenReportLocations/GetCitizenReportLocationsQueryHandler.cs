using Application.Common.Interfaces.Info;
using Application.Info.Common;

namespace Application.Reports.Queries.GetCitizenReportLocations;

internal class GetCitizenReportLocationsQueryHandler(IInfoService infoService) : IRequestHandler<GetCitizenReportLocationsQuery, Result<InfoModel>>
{
    public async Task<Result<InfoModel>> Handle(GetCitizenReportLocationsQuery request, CancellationToken cancellationToken)
    {
        var result = await infoService.GetCitizenReportLocations(request.InstanceId, request.Roles);
        return result;
    }
}
