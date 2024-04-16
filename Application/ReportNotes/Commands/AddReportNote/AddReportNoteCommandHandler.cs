using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.ReportNotes.Common;
using Domain.Models.Relational.ReportAggregate;
using Mapster;
using SharedKernel.Successes;

namespace Application.ReportNotes.Commands.AddReportNote;

internal class AddReportNoteCommandHandler(
    IReportNoteRepository reportNoteRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddReportNoteCommand, Result<ReportNoteResult>>
{
    public async Task<Result<ReportNoteResult>> Handle(AddReportNoteCommand request, CancellationToken cancellationToken)
    {
        var note = ReportNote.Create(request.UserId, request.ReportId, request.Text);
        reportNoteRepository.Insert(note);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(note.Adapt<ReportNoteResult>(), RegistrationSuccess.ReportNote);
    }
}
 