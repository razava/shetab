using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Categories.Queries.GetStaffCategories;

internal class GetStaffCategoriesQueryHandler(ICategoryRepository categoryRepository, IUserRepository userRepository) : IRequestHandler<GetStaffCategoriesQuery, Result<Category>>
{
    public async Task<Result<Category>> Handle(GetStaffCategoriesQuery request, CancellationToken cancellationToken)
    {
        var opCategories = await userRepository.GetUserCategoriesAsync(request.UserId);

        var allCategories = (await categoryRepository.GetAsync(p => 
            p.ShahrbinInstanceId == request.InstanceId
            && p.IsDeleted == false
            , false
            , null
            , "Categories")).ToList();

        var result = allCategories;

        if (opCategories.Any())
        {

            var filteredLeafs = allCategories.Where(c => !c.Categories.Any() && opCategories.Contains(c.Id)).ToList();
            var filteredLevel = filteredLeafs.Select(e => e.ParentId).ToList();
            var filteredCategoriesIds = filteredLeafs.Select(fl => fl.Id).ToList();

            while (true)
            {
                var tmp = allCategories.Where(ac => ac.ParentId != null && filteredLevel.Contains(ac.Id)).ToList();
                if (!tmp.Any())
                    break;
                filteredCategoriesIds.AddRange(tmp.Select(t => t.Id).ToList());
                filteredLevel = tmp.Select(fl => fl.ParentId).ToList();
            }

            //allCategories = allCategories.RemoveAll(c => !filteredCategoriesIds.Contains(c.Id));
            var filteredCategories = allCategories.Where(c => filteredCategoriesIds.Contains(c.Id)).ToList();
            var root = allCategories.Where(r => r.ParentId == null).SingleOrDefault();
            filteredCategories.Add(root);

            result = filteredCategories;
        }


        result.ForEach(x => x.Categories = result.Where(c => c.ParentId == x.Id).ToList());

        var categoryTree = allCategories.Where(r => r.ParentId == null).Single();

        return categoryTree;

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