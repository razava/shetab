using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Processes.Commands.UpdateProcessCommand;

internal class UpdateProcessCommandHandler : IRequestHandler<UpdateProcessCommand, bool>
{
    private readonly IProcessRepository _processRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProcessCommandHandler(IProcessRepository processRepository, IUnitOfWork unitOfWork)
    {
        _processRepository = processRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateProcessCommand request, CancellationToken cancellationToken)
    {
        await _processRepository.UpdateTypicalProcess(request.Id, request.Code, request.Title, request.ActorIds);
        await _unitOfWork.SaveAsync();
        return true;
    }
}
