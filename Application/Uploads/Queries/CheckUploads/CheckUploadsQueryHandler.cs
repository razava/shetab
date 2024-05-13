using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Application.Uploads.Queries.CheckUploads;

internal sealed class CheckUploadsQueryHandler(
    IUploadRepository uploadRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CheckUploadsQuery, Result<List<Guid>>>
{
    public async Task<Result<List<Guid>>> Handle(CheckUploadsQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext;
        List<Media> medias = new List<Media>();

        var attachments = (await uploadRepository
                .GetAsync(u => request.AttachmentIds.Contains(u.Id) && u.UserId == request.UserId))
                .ToList() ?? new List<Upload>();
        if (request.AttachmentIds.Count != attachments.Count)
        {
            return AttachmentErrors.AttachmentsFailure;
        }
        attachments.ForEach(a => a.IsUsed = true);

        context.Set<Upload>().AttachRange(attachments);
        await unitOfWork.SaveAsync();

        return attachments.Select(m => m.Id).ToList();
    }
}
