using Domain.Models.Relational;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IOrganizationalUnitRepository : IGenericRepository<OrganizationalUnit>
{
    public Task<bool> PhysicalDelete(int id);
    public Task<T?> GetOrganizationalUnitById<T>(int id, Expression<Func<OrganizationalUnit, T>> selector);
    public Task<T?> GetOrganizationalUnitByUserId<T>(string userId, Expression<Func<OrganizationalUnit, T>> selector);
    public Task<List<T>> GetOrganizationalUnits<T>(
        int instanceId, Expression<Func<OrganizationalUnit, bool>>? filter, Expression<Func<OrganizationalUnit, T>> selector);
}
