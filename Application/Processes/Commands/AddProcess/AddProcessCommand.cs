using Application.Processes.Common;

namespace Application.Processes.Commands.AddProcess;

public record AddProcessCommand(int InstanceId,
    string Code,
    string Title,
    List<int> ActorIds) : IRequest<Result<GetProcessResponse>>;