using Domain.Models.Relational;
using MediatR;

namespace Application.QuickAccesses.Queries.GetQuickAccesses;

public record GetQuickAccessesQuery(int InstanceId, List<string>? RoleNames = null) : IRequest<List<QuickAccess>>;
