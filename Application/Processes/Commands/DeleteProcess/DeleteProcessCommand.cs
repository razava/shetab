namespace Application.Processes.Commands.DeleteProcess;

public record DeleteProcessCommand(int Id) : IRequest<Result<bool>>;
