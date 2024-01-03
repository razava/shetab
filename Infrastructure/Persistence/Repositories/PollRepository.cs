using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Users.Common;
using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.PollAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PollRepository : IPollRepository
{
    private readonly ApplicationDbContext _context;
    public PollRepository(
        ApplicationDbContext dbContext)
    {       
        _context = dbContext;
    }

    public void Add(Poll poll)
    {
        _context.Set<Poll>().Add(poll);
    }

    public async Task<Poll> GetById(int id, string userId)
    {
        var result = await _context.Set<Poll>()
            .Where(p => p.Id == id)
            .Include(p => p.Choices)
            .Include(p => p.Answers.Where(pa => pa.UserId == userId))
            .SingleOrDefaultAsync();
        if (result is null)
            throw new NotFoundException("نظرسنجی");
        return result;
    }

    public async Task<Poll> GetById(int id)
    {
        var result = await _context.Set<Poll>()
            .Where(p => p.Id == id)
            .Include(p => p.Choices)
            .SingleOrDefaultAsync();
        if (result is null)
            throw new NotFoundException("نظرسنجی");
        return result;
    }

    public void Update(Poll poll)
    {
        _context.Set<Poll>().Update(poll);
    }
}
