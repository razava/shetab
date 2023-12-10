using Domain.Models.Relational.ProcessAggregate;
using MediatR;
using static Application.Processes.Queries.GetProcessByIdQuery.GetProcessByIdQueryHandler;

namespace Application.Processes.Queries.GetProcessByIdQuery;

public record GetProcessByIdQuery(int Id) : IRequest<GetProcessByIdResponse>;
