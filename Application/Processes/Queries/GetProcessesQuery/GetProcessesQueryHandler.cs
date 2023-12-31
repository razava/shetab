using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Processes.Queries.GetProcessesQuery;

internal class GetProcessesQueryHandler : IRequestHandler<GetProcessesQuery, List<Process>>
{
    private readonly IProcessRepository _processRepository;

    public GetProcessesQueryHandler(IProcessRepository processRepository)
    {
        _processRepository = processRepository;
    }

    public async Task<List<Process>> Handle(GetProcessesQuery request, CancellationToken cancellationToken)
    {
        var result = await _processRepository.GetAsync(p =>
        ((request.FilterModel == null || request.FilterModel.Query == null) || p.Title.Contains(request.FilterModel.Query))
        , false);
        if (result is null)
            return new List<Process>();
        return result.ToList();
    }
}
