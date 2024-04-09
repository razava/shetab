using Application.Common.Interfaces.Persistence;

namespace Application.Reports.Queries.GetReportById;

public sealed record GetHistoryQuery(
    Guid Id,
    string UserId,
    int InstanceId) : IRequest<Result<List<HistoryResponse>>>;

