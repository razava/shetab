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

        //todo : .............. query shoud be pre processed for relate to current user *********

        switch (code)
        {
            case 1:
                result = await infoService.GetUsersStatistics(request.InstanceId);
                break;
            case 2:
                result = await infoService.GetReportsStatistics(request.InstanceId);
                break;
            case 3:
                result = await infoService.GetTimeStatistics(request.InstanceId);
                break;
            case 102:
                result = await infoService.GetReportsStatusPerCategory(request.InstanceId, request.Parameter);
                break;
            case 104:
                result = await infoService.GetReportsStatusPerRegion(request.InstanceId);
                break;
            case 203:
                result = await infoService.GetRepportsTimeByExecutive(request.InstanceId);
                break;
            case 204:
                result = await infoService.GetRepportsTimeByRegion(request.InstanceId);
                break;
            default:
                break;
        }



        return result;
    }
}
