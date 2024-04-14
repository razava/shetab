using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.Processes.Common;
using Mapster;
using SharedKernel.Successes;

namespace Application.Processes.Commands.AddProcess;

internal class AddProcessCommandHandler(IProcessRepository processRepository, IUnitOfWork unitOfWork) 
    : IRequestHandler<AddProcessCommand, Result<GetProcessResponse>>
{
    public async Task<Result<GetProcessResponse>> Handle(AddProcessCommand request, CancellationToken cancellationToken)
    {
        var process = await processRepository.AddTypicalProcess(request.InstanceId, request.Code, request.Title, request.ActorIds);
        await unitOfWork.SaveAsync();
        if (process == null)
            return CreationFailedErrors.Process;

        return ResultMethods.GetResult(process.Adapt<GetProcessResponse>(), CreationSuccess.Process);
    }
}
