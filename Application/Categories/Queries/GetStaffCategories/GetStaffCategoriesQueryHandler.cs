using Application.Categories.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Queries.GetStaffCategories;

internal class GetStaffCategoriesQueryHandler(
    ICategoryRepository categoryRepository) : IRequestHandler<GetStaffCategoriesQuery, Result<CategoryResponse>>
{
    public async Task<Result<CategoryResponse>> Handle(GetStaffCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await categoryRepository.GetStaffCategories(request.InstanceId, request.UserId, request.Roles);
        return result.Adapt<CategoryResponse>();

    }
}


/*
 
var opCategories = await userRepository.GetUserCategoriesAsync(request.UserId);

        var result = (await categoryRepository.GetAsync(p => 
            p.ShahrbinInstanceId == request.InstanceId
            && p.IsDeleted == false
            && (!opCategories.Any() || opCategories.Contains(p.Id))
            , false)).ToList();

        var parentIds = result.Select(c => c.ParentId).ToList();

        var parents = (await categoryRepository.GetAsync(e => parentIds.Contains(e.Id), false)).ToList();

        var root = await categoryRepository.GetSingleAsync(c => 
        c.ParentId == null && c.ShahrbinInstanceId == request.InstanceId, false);
        result.AddRange(parents);
        result.Add(root);
        

        return result.Distinct().ToList();

 */