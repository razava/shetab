using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ProcessAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Processes.Queries.GetProcessesQuery;

internal class GetProcessesQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetProcessesQuery, Result<List<Process>>>
{
    public async Task<Result<List<Process>>> Handle(GetProcessesQuery request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<Process>()
            .Where(p => p.ShahrbinInstanceId == request.InstanceId && p.IsDeleted == false);
        if (request.FilterModel?.Query is not null)
            query = query.Where(p => p.Title.Contains(request.FilterModel.Query));
        
        var result = await query.AsNoTracking().ToListAsync();

        if (result is null)
            return new List<Process>();
        return result.ToList();
    }
}
