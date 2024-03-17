using Domain.Models.Relational.Common;

namespace Application.Configurations.Queries.ShahrbinInstanceById;

public record ShahrbinInstanceByIdQuery(int InstanceId) : IRequest<Result<ShahrbinInstance>>;
