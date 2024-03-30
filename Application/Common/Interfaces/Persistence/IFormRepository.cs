using Domain.Models.Relational.ReportAggregate;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IFormRepository : IGenericRepository<Form>
{
    public Task<List<T>> GetForms<T>(int instanceId, Expression<Func<Form, T>> selector); 
    public Task<T?> GetFormById<T>(Guid id, Expression<Func<Form, T>> selector); 
}
