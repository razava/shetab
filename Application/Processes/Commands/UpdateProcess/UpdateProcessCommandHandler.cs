using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using SharedKernel.Successes;

namespace Application.Processes.Commands.UpdateProcess;

internal class UpdateProcessCommandHandler(IProcessRepository processRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProcessCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(UpdateProcessCommand request, CancellationToken cancellationToken)
    {
        await processRepository.UpdateTypicalProcess(request.Id, request.Code, request.Title, request.ActorIds);
        await unitOfWork.SaveAsync();
        return ResultMethods.GetResult(true, UpdateSuccess.Process);
    }
}
