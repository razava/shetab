using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.AcceptByOperator;

internal sealed class AcceptByOperatorCommandHandler(
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository,
    ICategoryRepository categoryRepository,
    IUploadRepository uploadRepository) : IRequestHandler<AcceptByOperatorCommand, Result<Report>>
{
    public async Task<Result<Report>> Handle(AcceptByOperatorCommand request, CancellationToken cancellationToken)
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
                    .GetAsync(u => deletedAttachments.Contains(u.Media.Id) && u.UserId == report.CitizenId))
                    .ToList() ?? new List<Upload>();
                
                attachments.ForEach(a => a.IsUsed = false);
                medias = medias.Where(m => request.Attachments.Contains(m.Id)).ToList();
        }

        report.Accept(
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
