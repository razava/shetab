using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;

namespace Application.Satisfactions.Commands.UpsertSatisfaction;

internal sealed class UpsertSatisfactionCommandHandler(
    ISatisfactionRepository satisfactionRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpsertSatisfactionCommand, Result<SatisfactionResponse>>
{
    public async Task<Result<SatisfactionResponse>> Handle(UpsertSatisfactionCommand request, CancellationToken cancellationToken)
    {
        Satisfaction? satisfaction;
        satisfaction = await satisfactionRepository.GetSingleAsync(s => s.ReportId == request.ReportId);
        if (satisfaction is null)
        {
            satisfaction = Satisfaction.Create(
                request.ReportId,
                request.UserId,
                request.Comment,
                request.Rating);

            satisfactionRepository.Insert(satisfaction);
        }
        else
        {
            satisfaction.Update(request.UserId, request.Comment, request.Rating);
            satisfactionRepository.Update(satisfaction);
        }

        await unitOfWork.SaveAsync();

        return new SatisfactionResponse(satisfaction.ActorId, satisfaction.Comments, satisfaction.Rating, satisfaction.History);
    }
}
