using Application.Common.Interfaces.Persistence;
using Application.Polls.Common;
using Domain.Models.Relational.Common;

namespace Application.Polls.Queries.GetPollsById;

internal sealed class GetPollsByIdQueryHandler(IPollRepository pollRepository) : IRequestHandler<GetPollsByIdQuery, Result<GetPollsResponse>>
{

    public async Task<Result<GetPollsResponse>> Handle(GetPollsByIdQuery request, CancellationToken cancellationToken)
    {
        var poll = await pollRepository.GetByIdNoTracking(request.Id, request.userId);

        if (poll == null)
            return NotFoundErrors.Poll;

        //var result = new List<GetPollsResponse>();
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
        var result = new GetPollsResponse(
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

        return result;

    }
}
