using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.UpdateByOperator;

internal sealed class UpdateByOperatorCommandHandler(
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository,
    ICategoryRepository categoryRepository,
    IUploadRepository uploadRepository) : IRequestHandler<UpdateByOperatorCommand, Result<Report>>
{
    public async Task<Result<Report>> Handle(UpdateByOperatorCommand request, CancellationToken cancellationToken)
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

        List<Media>? medias = null;
        if(request.Attachments is not null)
        {
            List<Upload> attachments = new List<Upload>();
            if (request.Attachments.Count > 0)
            {
                attachments = (await uploadRepository
                .GetAsync(u => request.Attachments.Contains(u.Id) && u.UserId == request.operatorId))
                .ToList() ?? new List<Upload>();
                if (request.Attachments.Count != attachments.Count)
                {
                    return AttachmentErrors.AttachmentsFailure;
                }
                attachments.ForEach(a => a.IsUsed = true);
                medias = attachments.Select(a => a.Media).ToList();
            }
        }
        
        report.Update(
            request.operatorId,
            category,
            request.Comments,
            address,
            medias,
            null);

        await unitOfWork.SaveAsync();

        return report;
    }
}
