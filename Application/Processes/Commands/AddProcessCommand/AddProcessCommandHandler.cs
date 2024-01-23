using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Processes.Commands.AddProcessCommand;

internal class AddProcessCommandHandler(IProcessRepository processRepository, IUnitOfWork unitOfWork) : IRequestHandler<AddProcessCommand, Result<Process>>
{
    public async Task<Result<Process>> Handle(AddProcessCommand request, CancellationToken cancellationToken)
    {
        var process = await processRepository.AddTypicalProcess(request.InstanceId, request.Code, request.Title, request.ActorIds);
        await unitOfWork.SaveAsync();
        if (process == null)
            return CreationFailedErrors.Process;

        return process;
    }
}
