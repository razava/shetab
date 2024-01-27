using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Configurations.Queries.ShahrbinInstanceManagement;

internal sealed class ShahrbinInstancesQueryHandler(IShahrbinInstanceRepository shahrbinInstanceRepository) : IRequestHandler<ShahrbinInstancesQuery, Result<List<ShahrbinInstance>>>
{
    public async Task<Result<List<ShahrbinInstance>>> Handle(ShahrbinInstancesQuery request, CancellationToken cancellationToken)
    {
        var result = await shahrbinInstanceRepository.GetAsync(null, false, o => o.OrderBy(q => q.Id));
        return result.ToList();
    }
}
