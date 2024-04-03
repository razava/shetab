using Application.Common.Interfaces.Persistence;
using Application.Polls.Common;
using Domain.Models.Relational.Common;

namespace Application.Polls.Queries.GetPolls;

internal class GetPollsQueryHandler(IPollRepository pollRepository) : IRequestHandler<GetPollsQuery, Result<List<GetPollsResponse>>>
{

    public async Task<Result<List<GetPollsResponse>>> Handle(GetPollsQuery request, CancellationToken cancellationToken)
    {
        var polls = await pollRepository.GetAll(request.UserId, request.ReturnAll)

        var result = new List<GetPollsResponse>();
        foreach (var poll in polls)
        {
            var choices = new List<PollChoiceResponse>();
            poll.Choices.ToList().ForEach(pc => choices.Add(new PollChoiceResponse(pc.Id, pc.ShortTitle, pc.Text, pc.Order)));
            PollAnswerResponse? answerResponse = null;
            if (poll.Answers.Any())
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