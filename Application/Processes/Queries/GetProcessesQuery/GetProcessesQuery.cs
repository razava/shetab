using Application.Common.FilterModels;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Processes.Queries.GetProcessesQuery;

public record GetProcessesQuery(QueryFilterModel? FilterModel = default!) : IRequest<List<Process>>;
