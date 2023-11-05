using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Configurations.Queries.ViolationTypes;

internal sealed class ViolationTypesQueryHandler : IRequestHandler<ViolationTypesQuery, List<ViolationType>>
{
    private readonly IViolationTypeRepository _violationTypeRepository;

    public ViolationTypesQueryHandler(IViolationTypeRepository violationTypeRepository)
    {
        _violationTypeRepository = violationTypeRepository;
    }

    public async Task<List<ViolationType>> Handle(ViolationTypesQuery request, CancellationToken cancellationToken)
    {
        var result = await _violationTypeRepository.GetAsync(null, false);
        return result.ToList();
    }
}
