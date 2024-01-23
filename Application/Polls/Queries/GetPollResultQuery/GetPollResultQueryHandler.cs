using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.PollAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Polls.Queries.GetPollResultQuery;

internal class GetPollResultQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPollResultQuery, Result<PollResultResponce>>
{

    public async Task<Result<PollResultResponce>> Handle(GetPollResultQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;
        var poll = await context.Set<Poll>()
            .Where(p => p.Id == request.PollId)
            .Include(p => p.Choices)
            .Select(p => new { Choices = p.Choices.ToList(), Count = p.Answers.LongCount() })
            .SingleOrDefaultAsync();
        if (poll is null)
            return NotFoundErrors.Poll;

        var choices = await context.Set<Poll>()
            .Where(p=>p.Id == request.PollId)
            .SelectMany(p => p.Answers.SelectMany(p => p.Choices))
            .GroupBy(pc => pc.Id)
            .Select(pacg => new { Id = pacg.Key, Count = pacg.LongCount() })
            .ToListAsync();


        var total = choices.Sum(p => p.Count);
        var choiceCount = new List<PollChoiceResult>();
        foreach (var choice in poll.Choices.OrderBy(c => c.Order))
        {
            var c = choices.SingleOrDefault(c => c.Id == choice.Id);
            double percentage;
            if(c is null)
            {
                percentage = 0;
            }
            else
            {
                percentage = (double)c.Count/total;
            }
            choiceCount.Add(new PollChoiceResult(choice.ShortTitle, percentage));
        }
        var result = new PollResultResponce(poll.Count, choiceCount);
        return result;
    }
}