using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.Polls.Queries.GetPollsQuery;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.PollAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Polls.Queries.GetPollsByIdQuery;

internal sealed class GetPollsByIdQueryHandler : IRequestHandler<GetPollsByIdQuery, GetPollsResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPollsByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<GetPollsResponse> Handle(GetPollsByIdQuery request, CancellationToken cancellationToken)
    {
        var context = _unitOfWork.DbContext;

        var poll = await context.Set<Poll>()
            .Where(p => p.Id == request.Id)
            .Include(p => p.Choices)
            .Include(p => p.Answers.Where(pa => pa.UserId == request.userId))
            .ThenInclude(pa => pa.Choices)
            .SingleOrDefaultAsync();

        if (poll == null)
            throw new NotFoundException("Poll");

        //var result = new List<GetPollsResponse>();
        var choices = new List<PollChoiceResponse>();
        poll.Choices.ToList().ForEach(pc => choices.Add(new PollChoiceResponse(pc.Id, pc.ShortTitle, pc.Text, pc.Order)));
        PollAnswerResponse? answerResponse = null;
        if (poll.Answers.Any())
        {
            if (poll.PollType == PollType.Descriptive)
                answerResponse = new PollAnswerResponse(null, poll.Answers.Single().Text);
            else
                answerResponse = new PollAnswerResponse(poll.Answers.Single().Choices.Select(pac => pac.Id).ToList(), null);
        }
        var result = new GetPollsResponse(poll.Id, poll.Title, poll.PollType, poll.Question, choices, poll.Status, answerResponse, poll.IsDeleted);

        return result;

    }
}
