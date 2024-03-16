using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.PollAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Polls.Queries.GetPollsQuery;

internal class GetPollsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPollsQuery, Result<List<GetPollsResponse>>>
{

    public async Task<Result<List<GetPollsResponse>>> Handle(GetPollsQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;

        var polls = await context.Set<Poll>()
            .Where(p => request.ReturnAll || p.IsDeleted == false)
            .Include(p => p.Choices)
            .Include(p => p.Answers.Where(pa => pa.UserId == request.UserId))
            .ThenInclude(pa => pa.Choices)
            .ToListAsync();

        var result = new List<GetPollsResponse>();
        foreach(var poll in polls)
        {
            var choices = new List<PollChoiceResponse>();
            poll.Choices.ToList().ForEach(pc => choices.Add(new PollChoiceResponse(pc.Id, pc.ShortTitle, pc.Text, pc.Order)));
            PollAnswerResponse? answerResponse = null;
            if(poll.Answers.Any())
            {
                if (poll.PollType == PollType.Descriptive)
                    answerResponse = new PollAnswerResponse(null, poll.Answers.Single().Text);
                else
                    answerResponse = new PollAnswerResponse(
                        poll.Answers.Single().Choices.Select(pac => new PollAnswerItemResponse(pac.Id)).ToList(),
                        null);
            }
            var pr = new GetPollsResponse(
                poll.Id,
                poll.Title,
                poll.PollType,
                poll.Question,
                choices,
                poll.Status,
                poll.Created,
                poll.Expiration,
                answerResponse,
                poll.IsDeleted);
            result.Add(pr);
        }

        return result;
    }
}