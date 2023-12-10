using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Processes.Queries.GetProcessesQuery;

public record GetProcessesQuery() : IRequest<List<Process>>;
