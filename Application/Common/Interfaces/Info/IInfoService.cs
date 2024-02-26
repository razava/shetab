using Application.Info.Common;

namespace Application.Common.Interfaces.Info;

public interface IInfoService
{
    Task<InfoModel> GetUsersStatistics(int instanceId);
    Task<InfoModel> GetReportsStatistics(int instanceId);
    Task<InfoModel> GetTimeStatistics(int instanceId);
    Task<InfoModel> GetReportsStatusPerRegion(int instanceId);
    Task<InfoModel> GetRepportsTimeByRegion(int instanceId);
    Task<InfoModel> GetRepportsTimeByExecutive(int instanceId);
    Task<InfoModel> GetReportsStatusPerCategory(int instanceId, string? parameter);
}
