using Domain.Models.Relational.Common;

namespace Application.Configurations.Queries.ShahrbinInstanceManagement;

public record ShahrbinInstancesQuery() : IRequest<Result<List<ShahrbinInstance>>>;
