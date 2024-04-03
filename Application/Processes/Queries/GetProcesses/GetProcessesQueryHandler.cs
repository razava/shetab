using Application.Common.Interfaces.Persistence;
using Application.Processes.Common;
using Domain.Models.Relational.ProcessAggregate;
using System.Linq.Expressions;

namespace Application.Processes.Queries.GetProcesses;

internal class GetProcessesQueryHandler(IProcessRepository processRepository)
    : IRequestHandler<GetProcessesQuery, Result<List<GetProcessListResponse>>>
{
    public async Task<Result<List<GetProcessListResponse>>> Handle(GetProcessesQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Process, bool>>? filter = p =>
            (request.FilterModel != null && request.FilterModel.Query != null) ?
            p.Title.Contains(request.FilterModel.Query) : true;

        var result = await processRepository.GetProcesses(
            request.InstanceId,
            GetProcessListResponse.GetSelector(),
            filter);

        return result.ToList();
    }
}
