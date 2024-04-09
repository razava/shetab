using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetNearestReports;

public sealed record GetNearestReportsQuery(
    PagingInfo PagingInfo,
    int InstanceId,
    string UserId,
    double Longitude,
    double Latitude) : IRequest<Result<PagedList<GetCitizenReportsResponse>>>;

