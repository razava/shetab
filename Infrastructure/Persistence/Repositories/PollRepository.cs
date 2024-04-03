using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Info.Common;
using Domain.Models.Relational.PollAggregate;
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

    public async Task<List<Poll>> GetAll(string userId, bool returnAll = false)
    {
        var result = await _context.Set<Poll>()
            .AsNoTracking()
            .Where(p => returnAll || p.IsDeleted == false)
            .Include(p => p.Choices)
            .Include(p => p.Answers.Where(pa => pa.UserId == userId))
            .ThenInclude(pa => pa.Choices)
            .ToListAsync();
        return result;
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

     
    public async Task<Poll> GetByIdNoTracking(int id, string userId)
    {
        var result = await _context.Set<Poll>()
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Include(p => p.Choices)
            .Include(p => p.Answers.Where(pa => pa.UserId == userId))
            .ThenInclude(pa => pa.Choices)
            .SingleOrDefaultAsync();
        if (result is null)
            throw new NotFoundException("نظرسنجی");
        return result;
    }

    public async Task<InfoModel> GetPollResult(int id)
    {
        var poll = await _context.Set<Poll>()
            .Where(p => p.Id == id)
            .Include(p => p.Choices)
            .Select(p => new { p.Title, Choices = p.Choices.ToList(), Count = p.Answers.LongCount() })
            .SingleOrDefaultAsync();
        if (poll is null)
            throw new NotFoundException("نظرسنجی");

        var choices = await _context.Set<Poll>()
            .Where(p => p.Id == id)
            .SelectMany(p => p.Answers.SelectMany(p => p.Choices))
            .GroupBy(pc => pc.Id)
            .Select(pacg => new { Id = pacg.Key, Count = pacg.LongCount() })
            .ToListAsync();

        var total = choices.Sum(p => p.Count);
        total = total == 0 ? 1 : total;

        var info = new InfoModel();
        info.Singletons = [new InfoSingleton(poll.Count.ToString(), "شرکت کنندگان", "")];
        var chart = new InfoChart(poll.Title, "", false, false);
        info.Charts = [chart];

        var serie = new InfoSerie(poll.Title, "");
        chart.Add(serie);

        foreach (var choice in poll.Choices.OrderBy(c => c.Order))
        {
            var c = choices.SingleOrDefault(c => c.Id == choice.Id);
            double percentage;
            if (c is null)
            {
                percentage = 0;
            }
            else
            {
                percentage = (double)c.Count / total;
            }
            serie.Add(choice.ShortTitle, (c?.Count ?? 0).ToString(), percentage.ToString());
        }
        return info;
    }

    public void Update(Poll poll)
    {
        _context.Set<Poll>().Update(poll);
    }
}
