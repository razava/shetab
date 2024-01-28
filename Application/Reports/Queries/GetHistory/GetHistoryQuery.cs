using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetReportById;

public sealed record GetHistoryQuery(
    Guid Id,
    string UserId,
    int InstanceId) : IRequest<Result<List<TransitionLog>>>;

