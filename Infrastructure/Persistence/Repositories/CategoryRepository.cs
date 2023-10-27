using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class CategoryRepository: GenericRepository<Category>, ICategoryRepository
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
}
