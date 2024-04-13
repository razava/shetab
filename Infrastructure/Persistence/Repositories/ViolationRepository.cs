using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class ViolationRepository : GenericRepository<Violation>, IViolationRepository
{
    private readonly ApplicationDbContext _context;
    public ViolationRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _context = dbContext;
    }

    public async Task<PagedList<T>> GetCommentViolationList<T>(int instanceId, Expression<Func<Comment, T>> selector, PagingInfo pagingInfo)
    {
        var query = _context.Comment
            .AsNoTracking()
            .Where(c => c.ShahrbinInstanceId == instanceId && c.ViolationCount > 0 && !c.IsViolationChecked)
            .Select(selector);

        return await PagedList<T>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
    }

    public async Task<PagedList<T>> GetCommentViolations<T>(Guid commentId, Expression<Func<Violation, T>> selector, PagingInfo pagingInfo)
    {
        var query = _context.Violation
            .AsNoTracking()
            .Where(v => v.CommentId == commentId)
            .Select(selector);

        return await PagedList<T>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
    }

    public async Task<PagedList<T>> GetReportViolationList<T>(int instanceId, Expression<Func<Report, T>> selector, PagingInfo pagingInfo)
    {
        var query = _context.Reports
            .AsNoTracking()
            .Where(r => r.ShahrbinInstanceId == instanceId && r.ViolationCount > 0 && !r.IsViolationChecked)
            .Select(selector);

        return await PagedList<T>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
    }

    public async Task<PagedList<T>> GetReportViolations<T>(Guid reportId, Expression<Func<Violation, T>> selector, PagingInfo pagingInfo)
    {
        var query = _context.Violation
            .AsNoTracking()
            .Where(v => v.ReportId == reportId)
            .Select(selector);

        return await PagedList<T>.ToPagedList(query, pagingInfo.PageNumber, pagingInfo.PageSize);
    }
}
