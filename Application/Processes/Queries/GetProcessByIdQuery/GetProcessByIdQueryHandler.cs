using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Processes.Queries.GetProcessByIdQuery;

internal class GetProcessByIdQueryHandler : IRequestHandler<GetProcessByIdQuery, Process>
{
    private readonly IProcessRepository _processRepository;

    public GetProcessByIdQueryHandler(IProcessRepository processRepository)
    {
        _processRepository = processRepository;
    }

    public async Task<Process> Handle(GetProcessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _processRepository.GetSingleAsync(p => p.Id == request.Id, false, "Actors");
        if (result is null)
            throw new Exception("Not found!");
        return result;
    }
}
