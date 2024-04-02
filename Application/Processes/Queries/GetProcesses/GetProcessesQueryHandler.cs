using Application.Common.Interfaces.Persistence;
using Application.Processes.Common;
using Domain.Models.Relational.ProcessAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Processes.Queries.GetProcesses;

internal class GetProcessesQueryHandler(IProcessRepository processRepository)
    : IRequestHandler<GetProcessesQuery, Result<List<GetProcessListResponse>>>
{
    public async Task<Result<List<GetProcessListResponse>>> Handle(GetProcessesQuery request, CancellationToken cancellationToken)
    {
        //var query = unitOfWork.DbContext.Set<Process>()
        //    .Where(p => p.ShahrbinInstanceId == request.InstanceId && p.IsDeleted == false);
        //if (request.FilterModel?.Query is not null)
        //    query = query.Where(p => p.Title.Contains(request.FilterModel.Query));
        //var result = await query.Select(GetProcessListResponse.GetSelector()).ToListAsync();

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
