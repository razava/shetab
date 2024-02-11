using Application.Categories.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Queries.GetStaffCategories;

internal class GetStaffCategoriesQueryHandler(
    IUnitOfWork unitOfWork) : IRequestHandler<GetStaffCategoriesQuery, Result<CategoryResponse>>
{
    public async Task<Result<CategoryResponse>> Handle(GetStaffCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<Category>()
            .Where(c => c.ShahrbinInstanceId == request.InstanceId
                        && !c.IsDeleted);

        if (request.Roles.Contains(RoleNames.Operator))
            query = query.Where(c => c.Users.Any(cu => cu.Id == request.UserId));

        var categories = await query
            .Include(c => c.Form)
            .AsNoTracking()
            .ToListAsync();

        categories.ForEach(x => x.Categories = categories.Where(c => c.ParentId == x.Id).ToList());
        var roots = categories.Where(x => x.ParentId == null).ToList();

        Category result;
        if (roots.Count == 1)
            result = roots[0];
        else
        {
            result = Category.Create(request.InstanceId, "", "ریشه", "", 0, -1, "", 0, 0);
            roots.Where(r => r.ParentId == null).ToList().ForEach(c => c.Parent = result);
        }
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