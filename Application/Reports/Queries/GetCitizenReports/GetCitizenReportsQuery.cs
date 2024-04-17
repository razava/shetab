using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetCitizenReports;

//todo : add instanceId nullable for use in admin
public sealed record GetCitizenReportsQuery(
    PagingInfo PagingInfo,
    string UserId,
    int? instanceId) : IRequest<Result<PagedList<GetCitizenReportsResponse>>>;

