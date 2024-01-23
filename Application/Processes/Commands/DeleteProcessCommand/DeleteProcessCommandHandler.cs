using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Processes.Commands.DeleteProcessCommand;

internal class DeleteProcessCommandHandler(IProcessRepository processRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteProcessCommand, Result<bool>>
{
    
    public async Task<Result<bool>> Handle(DeleteProcessCommand request, CancellationToken cancellationToken)
    {
        await processRepository.LogicalDelete(request.Id);
        await unitOfWork.SaveAsync();

        return true;
    }
}
