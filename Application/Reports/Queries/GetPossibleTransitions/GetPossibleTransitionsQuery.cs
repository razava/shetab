using Application.Reports.Common;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetPossibleTransitions;

public sealed record GetPossibleTransitionsQuery(
    Guid reportId,
    string userId,
    int instanceId) : IRequest<List<PossibleTransitionResponse>>;

