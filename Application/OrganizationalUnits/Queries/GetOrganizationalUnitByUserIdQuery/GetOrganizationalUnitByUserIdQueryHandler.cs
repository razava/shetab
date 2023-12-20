using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitByUserIdQuery;

internal class GetOrganizationalUnitByUserIdQueryHandler : IRequestHandler<GetOrganizationalUnitByUserIdQuery, OrganizationalUnit>
{
    private readonly IOrganizationalUnitRepository _organizationalUnitRepository;

    public GetOrganizationalUnitByUserIdQueryHandler(IOrganizationalUnitRepository organizationalUnitRepository)
    {
        _organizationalUnitRepository = organizationalUnitRepository;
    }

    public async Task<OrganizationalUnit> Handle(GetOrganizationalUnitByUserIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _organizationalUnitRepository.GetSingleAsync(
            ou => ou.UserId == request.UserId,
            false);
        if (result is null)
            throw new NotFoundException("Organizational Unit");
        return result;
    }
}
