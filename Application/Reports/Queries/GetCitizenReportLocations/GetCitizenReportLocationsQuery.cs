using Application.Info.Common;

namespace Application.Reports.Queries.GetCitizenReportLocations;

public sealed record GetCitizenReportLocationsQuery(
    int InstanceId,
    List<string> Roles) : IRequest<Result<InfoModel>>;
