using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Processes.Queries.GetProcessByIdQuery;

public record GetProcessByIdQuery(int Id) : IRequest<Process>;
