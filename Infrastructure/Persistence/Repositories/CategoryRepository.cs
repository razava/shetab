using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    private readonly IProcessRepository _processRepository;

    public CategoryRepository(
        ApplicationDbContext dbContext, 
        IProcessRepository processRepository) : base(dbContext)
    {
        _processRepository = processRepository;
    }

    public async Task<Category?> GetByIDAsync(int id, bool trackChanges = true)
    {
        var result = await base.GetSingleAsync(c => c.Id == id, trackChanges);
        if (result is null)
            return null;
        if(result.ProcessId is not null)
            result.Process = await _processRepository.GetByIDAsync(result.ProcessId.Value);

        return result;
    }


    public async Task<Category?> GetById(int id, string includeProperties = "", bool trackChanges = false)
    {
        var query = context.Category.Where(c => c.Id == id);

        if(!trackChanges)
            query = query.AsNoTracking();

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        var result = await query.SingleOrDefaultAsync();
        return result;

    }


    public async Task<Category?> GetByIDNoTrackingAsync(object id)
    {
        var categoryId = (int)id;
        var result = await context.Category.Where(c => c.Id == categoryId)
            .Include(c => c.Process)
            .ThenInclude(p => p!.Stages)
            .Include(c => c.Process)
            .ThenInclude(p => p!.Transitions)
            .Include(c => c.Process)
            .ThenInclude(p => p!.RevisionUnit)
            
            .AsNoTracking()
            .SingleOrDefaultAsync();

        return result;
    }

    public async Task<Category?> GetStaffCategories(int instanceId, string userId, List<string> roles)
    {
        var query = context.Set<Category>()
            .Where(c => c.ShahrbinInstanceId == instanceId);

        if (roles.Contains(RoleNames.Operator))
            query = query.Where(c => c.Users.Any(cu => cu.Id == userId));

        var categories = await query
            .Include(c => c.Form)
            .AsNoTracking()
            .ToListAsync();

        var orphanCategories = categories.Where(c => c.ParentId != null && !categories.Any(p => p.Id == c.ParentId)).ToList();
        orphanCategories.ForEach(c => c.ParentId = null);

        categories.ForEach(x => 
        { 
            var children = categories.Where(c => c.ParentId == x.Id && !c.IsDeleted).ToList();
            x.Categories = children;
            children.ForEach(child => child.ParentId = x.Id);
        });
        
        categories.RemoveAll(x => x.IsDeleted);

        var roots = categories.Where(x => x.ParentId == null).ToList();

        Category result;
        if (!roots.Any())
        {
            return null;
        }
        else if (roots.Count == 1)
            result = roots[0];
        else
        {
            result = roots.Where(x => x.CategoryType == CategoryType.Root).First();
            roots.Remove(result);
            roots.Where(r => r.ParentId == null).ToList().ForEach(c => { c.Parent = result; result.Categories.Add(c); });
        }

        return result;
    }



    public async Task<Category?> GetCategories(int instanceId, List<string>? roles, bool returnAll)
    {
        var query = context.Category
            .Where(c => c.ShahrbinInstanceId == instanceId
                        && (returnAll || !c.IsDeleted));

        if (roles is not null)
            query = query.Where(c => roles.Contains(c.Role.Name!));

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
            result = Category.CreateDummy("ریشه", "");
            roots.Where(r => r.ParentId == null).ToList().ForEach(c => c.Parent = result);
        }

        return result;
    }


}
