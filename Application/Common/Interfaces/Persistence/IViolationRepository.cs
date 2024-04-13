using Domain.Models.Relational;
using Domain.Models.Relational.ReportAggregate;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IViolationRepository : IGenericRepository<Violation>
{
    public Task<PagedList<T>> GetCommentViolations<T>(Guid commentId, Expression<Func<Violation, T>> selector, PagingInfo pagingInfo);
    public Task<PagedList<T>> GetCommentViolationList<T>(int instanceId, Expression<Func<Comment, T>> selector, PagingInfo pagingInfo);
    public Task<PagedList<T>> GetReportViolations<T>(Guid reportId, Expression<Func<Violation, T>> selector, PagingInfo pagingInfo);
    public Task<PagedList<T>> GetReportViolationList<T>(int instanceId, Expression<Func<Report, T>> selector, PagingInfo pagingInfo);

}
