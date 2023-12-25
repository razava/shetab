using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Primitives;
using MediatR;
using System.Linq.Expressions;

namespace Application.Categories.Queries.GetCategory;

internal sealed class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, List<Category>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        //TODO: perform required filtering
        //Expression<Func<Category, bool>> filter = p => (request.FilterModel!= null && request.FilterModel.Query != null) ? p.Title.Contains(request.FilterModel.Query) : true;
        //...filter in this layer just return that category satisfied condition, not parents and root for tree structure in response.

        var result = await _categoryRepository.GetAsync(p => p.ShahrbinInstanceId == request.InstanceId 
            //&& ((request.FilterModel == null || request.FilterModel.Query == null) || p.Title.Contains(request.FilterModel.Query))
            && (request.ReturnAll || p.IsDeleted == false), false);

        return result.ToList();
    }
}
