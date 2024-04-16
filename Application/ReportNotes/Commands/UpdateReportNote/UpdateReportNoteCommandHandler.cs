using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using SharedKernel.Successes;

namespace Application.ReportNotes.Commands.UpdateReportNote;

internal class UpdateReportNoteCommandHandler(
    IReportNoteRepository reportNoteRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateReportNoteCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateReportNoteCommand request,
        CancellationToken cancellationToken)
    {
        var note = await reportNoteRepository.GetSingleAsync(r => r.Id == request.Id 
            && r.UserId == request.UserId);
        if (note is null)
            return NotFoundErrors.News;

        note.Update(request.Text);
        reportNoteRepository.Update(note);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(true, UpdateSuccess.ReportNote);
    }
}
