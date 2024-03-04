using Application.Common.FilterModels;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Processes.Queries.GetProcessesQuery;

public record GetProcessesQuery(
    int InstanceId,
    QueryFilterModel? FilterModel = default!) : IRequest<Result<List<Process>>>;
