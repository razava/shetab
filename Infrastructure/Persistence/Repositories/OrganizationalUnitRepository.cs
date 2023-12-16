using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;

namespace Infrastructure.Persistence.Repositories;

public class OrganizationalUnitRepository : GenericRepository<OrganizationalUnit>, IOrganizationalUnitRepository
{
    public OrganizationalUnitRepository(
        ApplicationDbContext dbContext) : base(dbContext)
    {       
    }
}
