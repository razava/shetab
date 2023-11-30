using MediatR;
using Domain.Models.Relational.Common;

namespace Application.Configurations.Queries.ShahrbinInstanceManagement;

public record ShahrbinInstancesQuery() : IRequest<List<ShahrbinInstance>>;
