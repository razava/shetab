using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.CreateReportByCitizen;

internal sealed class CreateReportByCitizenCommandHandler(
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository,
    ICategoryRepository categoryRepository,
    IUploadRepository uploadRepository) 
    : IRequestHandler<CreateReportByCitizenCommand, Result<GetReportByIdResponse>>
{
    
    public async Task<Result<GetReportByIdResponse>> Handle(CreateReportByCitizenCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIDAsync(request.CategoryId);
        if(category is null)
        {
            //TODO: Handle this error
            return NotFoundErrors.Category;
        }
        var address = request.Address.GetAddress();
        //address.Location = new NetTopologySuite.Geometries.Point(request.Address.Longitude, request.Address.Latitude);

        List<Media> medias = new List<Media>();
        if (request.Attachments is not null)
        {
            List<Upload> attachments = new List<Upload>();
            if (request.Attachments.Count > 0)
            {
                attachments = (await uploadRepository
                .GetAsync(u => request.Attachments.Contains(u.Id) && u.UserId == request.CitizenId))
                .ToList() ?? new List<Upload>();
                if (request.Attachments.Count != attachments.Count)
                {
                    return AttachmentErrors.AttachmentsFailure;
                }
                attachments.ForEach(a => a.IsUsed = true);
                medias = attachments.Select(a => a.Media).ToList();
            }
        }

        var report = Report.NewByCitizen(
            request.CitizenId,
            request.PhoneNumber,
            category,
            request.Comments,
            address,
            medias,
            Visibility.EveryOne,
            Priority.Normal,
            request.IsIdentityVisible);

        reportRepository.Insert(report);
        await unitOfWork.SaveAsync();

        //TODO: Inform related users not all
        //await _hub.Clients.All.Update();
        
        return GetReportByIdResponse.FromReport(report);
    }
}
