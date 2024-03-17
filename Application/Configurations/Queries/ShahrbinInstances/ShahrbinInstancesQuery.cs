using Domain.Models.Relational.Common;

namespace Application.Configurations.Queries.ShahrbinInstances;

public record ShahrbinInstancesQuery() : IRequest<Result<List<ShahrbinInstance>>>;
