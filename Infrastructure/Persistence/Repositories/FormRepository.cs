using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories;

public class FormRepository : GenericRepository<Form>, IFormRepository
{
    private readonly ApplicationDbContext _dbContext;
    public FormRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetFormById<T>(Guid id, Expression<Func<Form, T>> selector)
    {
        var result = await _dbContext.Form.AsNoTracking()
            .Where(f => f.Id == id)
            .Select(selector)
            .SingleOrDefaultAsync();

        return result;    
    }

    public async Task<List<T>> GetForms<T>(int instanceId, Expression<Func<Form, T>> selector)
    {
        var result = await _dbContext.Form.AsNoTracking()
            .Where(f => f.ShahrbinInstanceId == instanceId && !f.IsDeleted)
            .Select(selector)
            .ToListAsync();

        return result;
    }

    public async Task LogicalDelete(Guid id)
    {
        var form = await _dbContext.Form
            .Where(f => f.Id == id)
            .SingleOrDefaultAsync();

        if (form == null) 
            throw new ServerNotFoundException("خطایی رخ داد", new FormNotFoundException());

        form.IsDeleted = true;
        _dbContext.Form.Attach(form);
        //await _dbContext.SaveChangesAsync();
    }
}
