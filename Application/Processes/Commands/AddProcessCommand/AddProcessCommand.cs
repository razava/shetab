using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Processes.Commands.AddProcessCommand;

public record AddProcessCommand(int InstanceId, string Code, string Title, List<int> ActorIds) : IRequest<Process>;