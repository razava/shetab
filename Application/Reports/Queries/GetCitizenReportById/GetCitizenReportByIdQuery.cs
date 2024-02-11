using Application.Reports.Common;

namespace Application.Reports.Queries.GetCitizenReportById;

public sealed record GetCitizenReportByIdQuery(
    Guid Id,
    string UserId) : IRequest<Result<GetReportByIdResponse>>;

