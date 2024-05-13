using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using SharedKernel.Successes;

namespace Application.Reports.Commands.UpdateByOperator;

internal sealed class UpdateByOperatorCommandHandler(
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository,
    ICategoryRepository categoryRepository,
    IUploadRepository uploadRepository) : IRequestHandler<UpdateByOperatorCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateByOperatorCommand request, CancellationToken cancellationToken)
    {
        var report = await reportRepository.GetByIDAsync(request.reportId);
        if (report == null)
            return NotFoundErrors.Report;

        Category? category = null;
        if (request.CategoryId is not null)
        {
            category = await categoryRepository.GetByIDAsync(request.CategoryId.Value);
            if (category is null)
                return NotFoundErrors.Category;

        }
        Address? address = null;
        if (request.Address is not null)
        {
            address = request.Address.GetAddress();
            //address.Location = new NetTopologySuite.Geometries.Point(request.Address.Longitude, request.Address.Latitude);
        }

        List<Media> medias = report.Medias.ToList();
        if (request.Attachments is not null && report.Medias.Count > request.Attachments.Count)
        {
            var deletedAttachments = report.Medias.Select(m => m.Id).ToList();
            deletedAttachments.RemoveAll(request.Attachments.Contains);

            var attachments = (await uploadRepository
                .GetAsync(u => deletedAttachments.Contains(u.Media.Id)))
                .ToList() ?? new List<Upload>();

            attachments.ForEach(a => a.IsUsed = false);
            medias = medias.Where(m => request.Attachments.Contains(m.Id)).ToList();
        }

        report.Update(
            request.operatorId,
            category,
            request.Comments,
            address,
            medias,
            request.Visibility,
            request.Priority);

        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(true, UpdateSuccess.Report);
    }
}
