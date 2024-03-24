using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;

namespace Application.Violations.Queries.GetReportViolationsList;

public record GetReportViolationsListQuery(int InstanceId, PagingInfo PagingInfo)
    : IRequest<Result<PagedList<GetReportsResponse>>>;
