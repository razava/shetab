using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Configurations.Queries.ShahrbinInstanceManagement;

internal sealed class ShahrbinInstancesQueryHandler : IRequestHandler<ShahrbinInstancesQuery, List<ShahrbinInstance>>
{
    private readonly IShahrbinInstanceRepository _shahrbinInstanceRepository;

    public ShahrbinInstancesQueryHandler(IShahrbinInstanceRepository shahrbinInstanceRepository)
    {
        _shahrbinInstanceRepository = shahrbinInstanceRepository;
    }

    public async Task<List<ShahrbinInstance>> Handle(ShahrbinInstancesQuery request, CancellationToken cancellationToken)
    {
        var result = await _shahrbinInstanceRepository.GetAsync(null, false, o => o.OrderBy(q => q.Id));
        return result.ToList();
    }
}
