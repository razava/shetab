using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.ReportAggregate;

namespace Infrastructure.Persistence.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProcessRepository _processRepository;

    public ReportRepository(
        ApplicationDbContext dbContext,
        ICategoryRepository categoryRepository,
        IProcessRepository processRepository) : base(dbContext)
    {
        _categoryRepository = categoryRepository;
        _processRepository = processRepository;
    }

    public async Task<Report?> GetByIDAsync(Guid id, bool trackChanges = true)
    {
        //TransitionLogs won't be modified, so there is no need to include them
        var result = await base.GetSingleAsync(r => r.Id == id, trackChanges, @"CurrentActor,Feedback");

        if (result is null)
            return null;

        result.Category = (await _categoryRepository.GetByIDAsync(result.CategoryId))!;
        result.Process = (await _processRepository.GetByIDAsync(result.ProcessId))!;

        return result;
    }
}
