using Application.Common.Interfaces.Persistence;

namespace Application.ReportNotes.Commands.DeleteReportNote;

internal class DeleteReportNoteCommandHandler(
    IReportNoteRepository reportNoteRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteReportNoteCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteReportNoteCommand request,
        CancellationToken cancellationToken)
    {
        var note = await reportNoteRepository.GetSingleAsync(r => r.Id == request.Id 
            && r.UserId == request.UserId);
        if (note is null)
            return NotFoundErrors.ReportNote;

        note.Delete();
        reportNoteRepository.Update(note);
        await unitOfWork.SaveAsync();

        return true;
    }
}
