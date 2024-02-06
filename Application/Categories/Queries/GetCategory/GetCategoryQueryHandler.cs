using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Queries.GetCategory;

internal sealed class GetCategoryQueryHandler(/*IUnitOfWork unitOfWork, */ICategoryRepository categoryRepository) : IRequestHandler<GetCategoryQuery, Result<List<Category>>>
{
   
    public async Task<Result<List<Category>>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform required filtering
        var result = await categoryRepository.GetAsync(p => p.ShahrbinInstanceId == request.InstanceId
            //&& ((request.FilterModel == null || request.FilterModel.Query == null) || p.Title.Contains(request.FilterModel.Query))
            && (request.ReturnAll || p.IsDeleted == false), false);

        //Expression<Func<Category, bool>> filter = p => (request.FilterModel!= null && request.FilterModel.Query != null) ? p.Title.Contains(request.FilterModel.Query) : true;

        
        /*
        var context = _unitOfWork.DbContext;
        var result = context.Set<Category>()
            .AsNoTracking()
            .Where(p => p.ShahrbinInstanceId == request.InstanceId && (request.ReturnAll || p.IsDeleted == false));

        if(request.FilterModel != null && request.FilterModel.Query != null)
        {
            result = result.Where(c => c.Title.Contains(request.FilterModel.Query))
                .Include(c => (c.Parent != null) ? c.Parent : null)
                .ThenInclude(r => (r.Parent != null) ? r.Parent : null)
                .ThenInclude(e => (e.Parent != null) ? e.Parent : null);
        }
        */
        return result.ToList();
    }
}
