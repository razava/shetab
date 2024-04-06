using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetRecentReports;

public sealed record GetRecentReportsQuery(
    PagingInfo PagingInfo,
    int InstanceId,
    string UserId,
    List<string> Roles) : IRequest<Result<PagedList<GetCitizenReportsResponse>>>;

