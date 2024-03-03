using Application.Info.Common;

namespace Application.Common.Interfaces.Info;

public interface IInfoService
{
    Task<InfoModel> GetUsersStatistics(int instanceId);
    Task<InfoModel> GetReportsStatistics(int instanceId);
    Task<InfoModel> GetTimeStatistics(int instanceId);

    Task<InfoModel> GetReportsStatusPerCategory(int instanceId, string? parameter);
    Task<InfoModel> GetReportsStatusPerExecutive(int instanceId);
    Task<InfoModel> GetReportsStatusPerContractor(int instanceId);
    Task<InfoModel> GetReportsStatusPerRegion(int instanceId);

    Task<InfoModel> GetReportsTimePerCategory(int instanceId, string? parameter);
    Task<InfoModel> GetReportsTimeByRegion(int instanceId);
    Task<InfoModel> GetRepportsTimeByExecutive(int instanceId);

    Task<InfoModel> GetRequestsPerOperator(int instanceId);
    Task<InfoModel> GetRequestsPerRegistrantType(int instanceId);
}
