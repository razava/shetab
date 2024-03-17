using Application.Common.Interfaces.Persistence;

namespace Application.Processes.Commands.DeleteProcess;

internal class DeleteProcessCommandHandler(IProcessRepository processRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteProcessCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeleteProcessCommand request, CancellationToken cancellationToken)
    {
        await processRepository.LogicalDelete(request.Id);
        await unitOfWork.SaveAsync();

        return true;
    }
}
