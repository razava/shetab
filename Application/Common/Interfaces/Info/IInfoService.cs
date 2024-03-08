using Application.Info.Common;
using Application.Info.Queries.GetInfoQuery;

namespace Application.Common.Interfaces.Info;

public interface IInfoService
{
    Task<InfoModel> GetUsersStatistics(GetInfoQueryParameters queryParameters);
    Task<InfoModel> GetReportsStatistics(GetInfoQueryParameters queryParameters);
    Task<InfoModel> GetTimeStatistics(GetInfoQueryParameters queryParameters);
    Task<InfoModel> GetSatisfactionStatistics(GetInfoQueryParameters queryParameters);
    Task<InfoModel> GetActiveCitizens(GetInfoQueryParameters queryParameters);

    Task<InfoModel> GetReportsStatusPerCategory(GetInfoQueryParameters queryParameters);
    Task<InfoModel> GetReportsStatusPerExecutive(GetInfoQueryParameters queryParameters);
    Task<InfoModel> GetReportsStatusPerContractor(GetInfoQueryParameters queryParameters);
    Task<InfoModel> GetReportsStatusPerRegion(GetInfoQueryParameters queryParameters);

    Task<InfoModel> GetReportsTimePerCategory(GetInfoQueryParameters queryParameters);
    Task<InfoModel> GetReportsTimeByRegion(GetInfoQueryParameters queryParameters);
    Task<InfoModel> GetRepportsTimeByExecutive(GetInfoQueryParameters queryParameters);

    Task<InfoModel> GetRequestsPerOperator(GetInfoQueryParameters queryParameters);
    Task<InfoModel> GetRequestsPerRegistrantType(GetInfoQueryParameters queryParameters);

    Task<InfoModel> GetLocations(GetInfoQueryParameters queryParameters);
}

public record GetInfoQueryParameters(
    int InstanceId,
    string UserId,
    List<string> Roles,
    string? Parameter,
    ReportFilters ReportFilters,
    List<ReportsToInclude>? ReportsToInclude,
    List<GeoPoint>? Geometry);
public record GeoPoint(double Longitude, double Latitude);