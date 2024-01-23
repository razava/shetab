using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitByUserIdQuery;

internal class GetOrganizationalUnitByUserIdQueryHandler(IOrganizationalUnitRepository organizationalUnitRepository) : IRequestHandler<GetOrganizationalUnitByUserIdQuery, Result<OrganizationalUnit>>
{

    public async Task<Result<OrganizationalUnit>> Handle(GetOrganizationalUnitByUserIdQuery request, CancellationToken cancellationToken)
    {
        var result = await organizationalUnitRepository.GetSingleAsync(
            ou => ou.UserId == request.UserId,
            false);
        if (result is null)
            throw new NotFoundException("واحد سازمانی");
        return result;
    }
}
