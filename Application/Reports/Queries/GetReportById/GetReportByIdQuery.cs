using Application.Reports.Common;

namespace Application.Reports.Queries.GetReportById;

public sealed record GetReportByIdQuery(
    Guid Id,
    string UserId,
    int InstanceId) : IRequest<Result<GetReportByIdResponse>>;

