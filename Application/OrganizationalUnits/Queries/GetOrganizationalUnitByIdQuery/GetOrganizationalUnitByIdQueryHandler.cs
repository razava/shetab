﻿using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnitByIdQuery;

internal class GetOrganizationalUnitByIdQueryHandler : IRequestHandler<GetOrganizationalUnitByIdQuery, OrganizationalUnit>
{
    private readonly IOrganizationalUnitRepository _organizationalUnitRepository;

    public GetOrganizationalUnitByIdQueryHandler(IOrganizationalUnitRepository organizationalUnitRepository)
    {
        _organizationalUnitRepository = organizationalUnitRepository;
    }

    public async Task<OrganizationalUnit> Handle(GetOrganizationalUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _organizationalUnitRepository.GetSingleAsync(
            ou => ou.Id == request.OrganizationalUnitId,
            false,
            "OrganizationalUnits");
        if (result is null)
            throw new NotFoundException("Organizational Unit");
        return result;
    }
}
