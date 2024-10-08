﻿using Application.Common.Interfaces.Persistence;
using Application.OrganizationalUnits.Common;
using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.OrganizationalUnits.Queries.GetOrganizationalUnits;

internal class GetOrganizationalUnitsQueryHandler(IOrganizationalUnitRepository organizationalUnitRepository) 
    : IRequestHandler<GetOrganizationalUnitsQuery, Result<List<GetOrganizationalUnitListResponse>>>
{

    public async Task<Result<List<GetOrganizationalUnitListResponse>>> Handle(GetOrganizationalUnitsQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<OrganizationalUnit, bool>>? filter = ou =>
            (request.FilterModel == null || request.FilterModel.Query == null || ou.Title.Contains(request.FilterModel.Query));

        var result = await organizationalUnitRepository.GetOrganizationalUnits(
            request.InstanceId, filter, GetOrganizationalUnitListResponse.GetSelector());

        return result.ToList();
    }
}
