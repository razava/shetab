using Domain.Models.Relational;
using MediatR;

namespace Application.Info.Queries.GetListChartQuery;

public record GetListChartQuery(int InstanceId, List<string> RoleNames) : IRequest<Result<List<Chart>>>;
