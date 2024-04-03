using Application.Common.Interfaces.Persistence;
using Application.ReportNotes.Common;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class ReportNoteRepository : GenericRepository<ReportNote>, IReportNoteRepository
{
    private readonly ApplicationDbContext _context; 
    public ReportNoteRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }

    public async Task<PagedList<T>> GetAllReportNotes<T>(string userId, Expression<Func<ReportNote, T>> selector, PagingInfo pagingInfo)
    {
        var query = _context.Set<ReportNote>()
            .AsNoTracking()
            .Where(r => r.UserId == userId && r.IsDeleted == false)
            .Select(selector);

        return await PagedList<T>.ToPagedList(
            query,
            pagingInfo.PageNumber,
            pagingInfo.PageSize);
    }

    public async Task<List<T>> GetReportNotes<T>(Guid reportId, string userId, Expression<Func<ReportNote, T>> selector)
    {
        var result = await _context.Set<ReportNote>()
            .AsNoTracking()
            .Where(r => r.ReportId == reportId && r.UserId == userId && r.IsDeleted == false)
            .Select(selector)
            .ToListAsync();
        return result;
    }
}
