using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using MediatR;

namespace Application.Processes.Queries.GetProcessByIdQuery;

internal partial class GetProcessByIdQueryHandler : IRequestHandler<GetProcessByIdQuery, GetProcessByIdResponse>
{
    private readonly IProcessRepository _processRepository;

    public GetProcessByIdQueryHandler(IProcessRepository processRepository)
    {
        _processRepository = processRepository;
    }

    public async Task<GetProcessByIdResponse> Handle(GetProcessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _processRepository.GetSingleAsync(p => p.Id == request.Id, false, "Stages,Stages.Actors");
        
        if (result is null)
            throw new NotFoundException("Process");

        var actorIds = result.Stages.Where(s => s.Name == "Executive").SingleOrDefault()!.Actors.Select(a => a.Id).ToList();

        return new GetProcessByIdResponse(result.Id, result.Code, result.Title, actorIds);
    }
}
