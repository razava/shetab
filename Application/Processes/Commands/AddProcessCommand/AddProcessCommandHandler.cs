using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Processes.Commands.AddProcessCommand;

internal class AddProcessCommandHandler : IRequestHandler<AddProcessCommand, bool>
{
    private readonly IProcessRepository _processRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProcessCommandHandler(IProcessRepository processRepository, IUnitOfWork unitOfWork)
    {
        _processRepository = processRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AddProcessCommand request, CancellationToken cancellationToken)
    {
        await _processRepository.AddTypicalProcess(request.InstanceId, request.Code, request.Title, request.ActorIds);
        await _unitOfWork.SaveAsync();
        return true;
    }
}
