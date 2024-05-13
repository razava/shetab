using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.Common;

namespace Application.Uploads.Commands.UpdateUploadsList;

internal sealed class UpdateUploadsListCommandHandler(
    IUploadRepository uploadRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateUploadsListCommand, Result<List<Guid>>>
{
    public async Task<Result<List<Guid>>> Handle(UpdateUploadsListCommand request, CancellationToken cancellationToken)
    {
        //todo : check access validation with userId & role
        var result = request.OldList;
        if (request.NewList is not null && request.OldList.Count > request.NewList.Count)
        {
            var deletedAttachments = request.OldList;
            deletedAttachments.RemoveAll(request.NewList.Contains);

            var attachments = (await uploadRepository
                .GetAsync(u => deletedAttachments.Contains(u.Media.Id)))
                .ToList() ?? new List<Upload>();

            attachments.ForEach(a => a.IsUsed = false);
            unitOfWork.DbContext.Set<Upload>().AttachRange(attachments);

            result = result.Where(m => request.NewList.Contains(m)).ToList();
        }
        await unitOfWork.SaveAsync();

        return result;
    }
}
