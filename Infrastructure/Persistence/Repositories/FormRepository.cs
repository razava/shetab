using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;

namespace Infrastructure.Persistence.Repositories;

public class FormRepository : GenericRepository<Form>, IFormRepository
{
    public FormRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        
    }
}
