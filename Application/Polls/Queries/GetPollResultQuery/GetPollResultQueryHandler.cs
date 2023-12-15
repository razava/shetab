using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.PollAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Polls.Queries.GetPollResultQuery;

internal class GetPollResultQueryHandler : IRequestHandler<GetPollResultQuery, PollResultResponce>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPollResultQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PollResultResponce> Handle(GetPollResultQuery request, CancellationToken cancellationToken)
    {
        var context = _unitOfWork.DbContext;
        var poll = await context.Set<Poll>()
            .Where(p => p.Id == request.PollId)
            .Select(p => new { p.Choices, Count = p.Answers.LongCount() })
            .SingleOrDefaultAsync();
        if (poll is null)
            throw new Exception("Not found!");

        var choices = await context.Set<PollAnswer>()
            .Where(pa => pa.PollId == request.PollId)
            .SelectMany(pa => pa.Choices.GroupBy(pac => pac.Id)
            .Select(pacg => new { Id = pacg.Key, Count = pacg.LongCount() }))
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