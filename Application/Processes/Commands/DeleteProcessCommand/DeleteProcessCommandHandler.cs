using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Processes.Commands.DeleteProcessCommand;

internal class DeleteProcessCommandHandler : IRequestHandler<DeleteProcessCommand, bool>
{
    private readonly IProcessRepository _processRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProcessCommandHandler(IProcessRepository processRepository, IUnitOfWork unitOfWork)
    {
        _processRepository = processRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteProcessCommand request, CancellationToken cancellationToken)
    {
        await _processRepository.LogicalDelete(request.Id);
        await _unitOfWork.SaveAsync();

        return true;
    }
}
