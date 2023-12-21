using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Processes.Commands.AddProcessCommand;

internal class AddProcessCommandHandler : IRequestHandler<AddProcessCommand, Process>
{
    private readonly IProcessRepository _processRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProcessCommandHandler(IProcessRepository processRepository, IUnitOfWork unitOfWork)
    {
        _processRepository = processRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Process> Handle(AddProcessCommand request, CancellationToken cancellationToken)
    {
        var process = await _processRepository.AddTypicalProcess(request.InstanceId, request.Code, request.Title, request.ActorIds);
        await _unitOfWork.SaveAsync();
        if (process == null)
            throw new CreationFailedException("Process");
        return process;
    }
}
