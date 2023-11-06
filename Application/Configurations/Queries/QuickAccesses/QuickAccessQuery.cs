using MediatR;
using Domain.Models.Relational;

namespace Application.Configurations.Queries.QuickAccesses;

public record QuickAccessQuery(int InstanceId) : IRequest<List<QuickAccess>>;
