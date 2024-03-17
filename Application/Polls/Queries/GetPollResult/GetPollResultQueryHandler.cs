using Application.Common.Interfaces.Persistence;
using Application.Info.Common;
using Domain.Models.Relational.PollAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Polls.Queries.GetPollResult;

internal class GetPollResultQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPollResultQuery, Result<InfoModel>>
{

    public async Task<Result<InfoModel>> Handle(GetPollResultQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;
        var poll = await context.Set<Poll>()
            .Where(p => p.Id == request.PollId)
            .Include(p => p.Choices)
            .Select(p => new { p.Title, Choices = p.Choices.ToList(), Count = p.Answers.LongCount() })
            .SingleOrDefaultAsync();
        if (poll is null)
            return NotFoundErrors.Poll;

        var choices = await context.Set<Poll>()
            .Where(p => p.Id == request.PollId)
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
}