using Domain.Models.Relational;
using MediatR;

namespace Application.QuickAccesses.Queries.GetQuickAccesses;

public record GetQuickAccessesQuery(
    int InstanceId,
    bool ReturnAll = false) : IRequest<List<QuickAccess>>;
