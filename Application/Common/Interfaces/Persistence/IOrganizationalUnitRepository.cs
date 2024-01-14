using Domain.Models.Relational;

namespace Application.Common.Interfaces.Persistence;

public interface IOrganizationalUnitRepository : IGenericRepository<OrganizationalUnit>
{
    public Task<bool> PhysicalDelete(int id);
}
