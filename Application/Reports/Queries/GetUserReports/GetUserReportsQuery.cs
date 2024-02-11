using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetUserReports;

public sealed record GetUserReportsQuery(
    PagingInfo PagingInfo,
    string UserId) : IRequest<Result<PagedList<GetCitizenReportsResponse>>>;

