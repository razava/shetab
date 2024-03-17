using Application.Common.Interfaces.Persistence;
using Application.Processes.Common;
using Domain.Models.Relational.ProcessAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Processes.Queries.GetProcesses;

internal class GetProcessesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetProcessesQuery, Result<List<GetProcessListResponse>>>
{
    public async Task<Result<List<GetProcessListResponse>>> Handle(GetProcessesQuery request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<Process>()
            .Where(p => p.ShahrbinInstanceId == request.InstanceId && p.IsDeleted == false);
        if (request.FilterModel?.Query is not null)
            query = query.Where(p => p.Title.Contains(request.FilterModel.Query));

        var result = await query.Select(GetProcessListResponse.GetSelector()).ToListAsync();

        return result.ToList();
    }
}
