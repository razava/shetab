using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Processes.Queries.GetProcessByIdQuery;

internal partial class GetProcessByIdQueryHandler(IProcessRepository processRepository) : IRequestHandler<GetProcessByIdQuery, Result<GetProcessByIdResponse>>
{

    public async Task<Result<GetProcessByIdResponse>> Handle(GetProcessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await processRepository.GetSingleAsync(p => p.Id == request.Id && p.IsDeleted == false , false, "Stages,Stages.Actors");

        if (result is null)
            return NotFoundErrors.Process;

        var actorIds = result.Stages.Where(s => s.Name == "Executive").SingleOrDefault()!.Actors.Select(a => a.Id).ToList();

        return new GetProcessByIdResponse(result.Id, result.Code, result.Title, actorIds);
    }
}
