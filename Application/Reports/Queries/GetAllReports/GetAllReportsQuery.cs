using Application.Common.FilterModels;
using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Reports.Queries.GetAllReports;

public sealed record GetAllReportsQuery(
    PagingInfo PagingInfo,
    int InstanceId,
    string UserId,
    List<string> Roles,
    FilterGetAllReportsModel? FilterGetReports = default!) 
    : IRequest<Result<PagedList<GetReportsResponse>>>;

