using Domain.Data;
using Domain.Models.Relational;

namespace Api.Services.Dal;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private ApplicationDbContext context;
    public UnitOfWork(ApplicationDbContext context)
    {
        this.context = context;
    }

    public ApplicationDbContext DbContext { get { return context; } }

    private GenericRepository<OrganizationalUnit>? organizationalUnitRepository;
    private GenericRepository<Category>? categoryRepository;
    private RequestHandlingDal? requestHandling;

    public GenericRepository<OrganizationalUnit> OrganizationalUnitRepository
    {
        get
        {
            if (organizationalUnitRepository == null)
            {
                organizationalUnitRepository = new GenericRepository<OrganizationalUnit>(context);
            }
            return organizationalUnitRepository;
        }
    }

    public GenericRepository<Category> CategoryRepository
    {
        get
        {
            if (categoryRepository == null)
            {
                categoryRepository = new GenericRepository<Category>(context);
            }
            return categoryRepository;
        }
    }

    
    public RequestHandlingDal RequestHandling
    {
        get
        {
            if (requestHandling == null)
            {
                requestHandling = new RequestHandlingDal(context);
            }
            return requestHandling;
        }
    }


    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }

    public void Save()
    {
        context.SaveChanges();
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}