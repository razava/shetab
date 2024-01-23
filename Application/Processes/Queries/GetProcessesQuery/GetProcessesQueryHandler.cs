using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;

namespace Application.Processes.Queries.GetProcessesQuery;

internal class GetProcessesQueryHandler(IProcessRepository processRepository) : IRequestHandler<GetProcessesQuery, Result<List<Process>>>
{
    public async Task<Result<List<Process>>> Handle(GetProcessesQuery request, CancellationToken cancellationToken)
    {
        var result = await processRepository.GetAsync(p => p.IsDeleted == false &&
        ((request.FilterModel == null || request.FilterModel.Query == null) || p.Title.Contains(request.FilterModel.Query))
        , false);
        if (result is null)
            return new List<Process>();
        return result.ToList();
    }
}
