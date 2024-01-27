using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Configurations.Queries.ViolationTypes;

internal sealed class ViolationTypesQueryHandler(IViolationTypeRepository violationTypeRepository) : IRequestHandler<ViolationTypesQuery, Result<List<ViolationType>>>
{

    public async Task<Result<List<ViolationType>>> Handle(ViolationTypesQuery request, CancellationToken cancellationToken)
    {
        var result = await violationTypeRepository.GetAsync(null, false);
        return result.ToList();
    }
}
