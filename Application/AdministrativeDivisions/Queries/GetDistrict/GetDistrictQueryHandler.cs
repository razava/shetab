﻿using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

internal sealed class GetDistrictQueryHandler(IDistrictRepository districtRepository) : IRequestHandler<GetDistrictQuery, Result<List<District>>>
{
    public async Task<Result<List<District>>> Handle(GetDistrictQuery request, CancellationToken cancellationToken)
    {
        var result = await districtRepository.GetAsync(p => p.CountyId == request.CountyId);

        return result.ToList();
    }
}
