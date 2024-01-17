using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Info.Queries.GetListChartQuery;

internal class GetListChartQueryHandler : IRequestHandler<GetListChartQuery, List<Chart>>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetListChartQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<List<Chart>> Handle(GetListChartQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DbContext.Set<Chart>()
            .AsNoTracking()
            .Where(c =>
            c.ShahrbinInstanceId == request.InstanceId && request.RoleNames.Any(r => c.Roles.Any(t => t.Name == r)))
            .OrderBy(e => e.Order)
            .ToListAsync();

        return result;
    }
}
