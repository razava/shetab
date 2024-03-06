using Application.Common.Interfaces.Info;
using Application.Info.Common;

namespace Application.Info.Queries.GetInfoQuery;

internal class GetInfoQueryHandler(
    IInfoService infoService) : IRequestHandler<GetInfoQuery, Result<InfoModel>>
{
    
    public async Task<Result<InfoModel>> Handle(GetInfoQuery request, CancellationToken cancellationToken)
    {
        var code = request.Code % 100000;
        InfoModel result = new InfoModel();

        var queryParameters = new GetInfoQueryParameters(request.InstanceId, request.UserId, request.Roles, request.Parameter);

        switch (code)
        {
            case 1:
                result = await infoService.GetUsersStatistics(queryParameters);
                break;
            case 2:
                result = await infoService.GetReportsStatistics(queryParameters);
                break;
            case 3:
                result = await infoService.GetTimeStatistics(queryParameters);
                break;
            case 4:
                result = await infoService.GetSatisfactionStatistics(queryParameters);
                break;
            case 5:
                result = await infoService.GetActiveCitizens(queryParameters);
                break;
            case 102:
                result = await infoService.GetReportsStatusPerCategory(queryParameters);
                break;
            case 103:
                result = await infoService.GetReportsStatusPerExecutive(queryParameters);
                break;
            case 104:
                result = await infoService.GetReportsStatusPerRegion(queryParameters);
                break;
            case 402:
                result = await infoService.GetReportsStatusPerContractor(queryParameters);
                break;
            case 202:
                result = await infoService.GetReportsTimePerCategory(queryParameters);
                break;
            case 203:
                result = await infoService.GetRepportsTimeByExecutive(queryParameters);
                break;
            case 204:
                result = await infoService.GetReportsTimeByRegion(queryParameters);
                break;
            case 302:
                result = await infoService.GetRequestsPerOperator(queryParameters);
                break;
            case 303:
                result = await infoService.GetRequestsPerRegistrantType(queryParameters);
                break;
            case 141:
                result = await infoService.GetLocations(queryParameters);
                break;
            default:
                break;
        }

        return result;
    }
}
