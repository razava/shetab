using Domain.Models.Relational;
using MediatR;

namespace Application.Info.Queries.GetListChart;

public record GetListChartQuery(int InstanceId, List<string> RoleNames) : IRequest<Result<List<Chart>>>;
