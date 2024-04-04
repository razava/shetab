using Application.Info.Queries.GetInfo;
using Domain.Models.Relational;
using Domain.Models.Relational.ReportAggregate;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IReportRepository : IGenericRepository<Report>
{
    public Task<Report?> GetByIDAsync(Guid id, bool trackChanges = true);
    public Task<T?> GetByIdSelective<T>(Guid id, Expression<Func<Report, bool>> filter, Expression<Func<Report, T>> selector);
    public Task<PagedList<T>> GetCitizenReports<T>(string userId, Expression<Func<Report, T>> selector, PagingInfo pagingInfo);
    public Task<PagedList<T>> GetRecentReports<T>(Expression<Func<Report, bool>> filter, Expression<Func<Report, T>> selector, PagingInfo pagingInfo);
    public Task<PagedList<T>> GetReports<T>(
        int instanceId,
        string userId,
        List<string> roles,
        string? fromRoleId,
        Expression<Func<Report, T>> selector,
        ReportFilters reportFilters,
        PagingInfo pagingInfo); 


}
 