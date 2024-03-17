using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Application.Configurations.Queries.ShahrbinInstanceById;

internal sealed class ShahrbinInstanceByIdQueryHandler(IShahrbinInstanceRepository shahrbinInstanceRepository)
    : IRequestHandler<ShahrbinInstanceByIdQuery, Result<ShahrbinInstance>>
{
    public async Task<Result<ShahrbinInstance>> Handle(ShahrbinInstanceByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await shahrbinInstanceRepository.GetFirstAsync(s => s.Id == request.InstanceId);
        if (result is null)
            return NotFoundErrors.Instance;

        return result;
    }
}
