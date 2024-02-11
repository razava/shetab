using Application.Categories.Common;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Queries.GetCategory;

internal sealed class GetCategoryQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetCategoryQuery, Result<CategoryResponse>>
{
   
    public async Task<Result<CategoryResponse>> Handle(
        GetCategoryQuery request,
        CancellationToken cancellationToken)
    {
        var query = unitOfWork.DbContext.Set<Category>()
            .Where(c => c.ShahrbinInstanceId == request.InstanceId
                        && (request.ReturnAll || !c.IsDeleted));

        if (request.Roles is not null)
            query = query.Where(c => request.Roles.Contains(c.Role.Name!));

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
