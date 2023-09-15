using Domain.Data;
using Domain.Models.Relational;

namespace Api.Services.Dal;

public interface IUnitOfWork
{
    public void Save();
    public Task SaveAsync();
    public ApplicationDbContext DbContext { get; }
    public GenericRepository<OrganizationalUnit> OrganizationalUnitRepository { get; }
    public GenericRepository<Category> CategoryRepository { get; }
    public RequestHandlingDal RequestHandling { get; }
}
