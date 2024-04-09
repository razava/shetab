using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetUserReports;

//todo : add instanceId nullable for use in admin
public sealed record GetUserReportsQuery(
    PagingInfo PagingInfo,
    string UserId,
    int? instanceId) : IRequest<Result<PagedList<GetCitizenReportsResponse>>>;

