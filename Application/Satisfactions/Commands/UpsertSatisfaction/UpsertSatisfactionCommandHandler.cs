using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using SharedKernel.Successes;

namespace Application.Satisfactions.Commands.UpsertSatisfaction;

internal sealed class UpsertSatisfactionCommandHandler(
    ISatisfactionRepository satisfactionRepository,
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository)
    : IRequestHandler<UpsertSatisfactionCommand, Result<SatisfactionResponse>>
{
    public async Task<Result<SatisfactionResponse>> Handle(UpsertSatisfactionCommand request, CancellationToken cancellationToken)
    {
        var report = await reportRepository.GetByIDAsync(request.ReportId);
        if (report == null)
            return NotFoundErrors.Report;

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

        report.UpdateFeedback(request.Rating);

        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(
            new SatisfactionResponse(
            satisfaction.ActorId,
            satisfaction.Comments,
            satisfaction.Rating,
            satisfaction.History),
            CreationSuccess.Satisfaction);
    }
}
