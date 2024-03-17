namespace Application.Processes.Commands.UpdateProcess;

public record UpdateProcessCommand(int Id,
    string? Code,
    string? Title,
    List<int>? ActorIds) : IRequest<Result<bool>>;

