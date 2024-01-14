using MediatR;

namespace Application.Processes.Commands.DeleteProcessCommand;

public record DeleteProcessCommand(int Id) : IRequest<bool>;
