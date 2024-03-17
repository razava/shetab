using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;


namespace Application.Info.Queries.GetListChart;

internal class GetListChartQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetListChartQuery, Result<List<Chart>>>
{

    public async Task<Result<List<Chart>>> Handle(GetListChartQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<Chart>()
            .AsNoTracking()
            .Where(c =>
            c.ShahrbinInstanceId == request.InstanceId && request.RoleNames.Any(r => c.Roles.Any(t => t.Name == r)))
            .OrderBy(e => e.Order)
            .ToListAsync();

        return result;
    }
}
