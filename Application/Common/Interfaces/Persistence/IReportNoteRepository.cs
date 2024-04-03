using Domain.Models.Relational.ReportAggregate;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IReportNoteRepository : IGenericRepository<ReportNote>
{
    public Task<List<T>> GetReportNotes<T>(Guid reportId, string userId, Expression<Func<ReportNote, T>> selector);
    public Task<PagedList<T>> GetAllReportNotes<T>(
        string userId, Expression<Func<ReportNote, T>> selector, PagingInfo pagingInfo);

}
