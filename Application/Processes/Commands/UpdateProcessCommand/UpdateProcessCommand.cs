using MediatR;

namespace Application.Processes.Commands.UpdateProcessCommand;

public record UpdateProcessCommand(int Id, string Code, string Title, List<int> ActorIds) : IRequest<bool>;