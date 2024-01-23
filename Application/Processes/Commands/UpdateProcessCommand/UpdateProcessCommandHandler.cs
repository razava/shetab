using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Processes.Commands.UpdateProcessCommand;

internal class UpdateProcessCommandHandler(IProcessRepository processRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProcessCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(UpdateProcessCommand request, CancellationToken cancellationToken)
    {
        await processRepository.UpdateTypicalProcess(request.Id, request.Code, request.Title, request.ActorIds);
        await unitOfWork.SaveAsync();
        return true;
    }
}
