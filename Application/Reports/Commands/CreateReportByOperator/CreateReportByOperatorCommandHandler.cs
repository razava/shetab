﻿using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.CreateReportByOperator;

internal sealed class CreateReportByOperatorCommandHandler(
    IUnitOfWork unitOfWork,
    IReportRepository reportRepository,
    ICategoryRepository categoryRepository,
    IUserRepository userRepository,
    IUploadRepository uploadRepository) : IRequestHandler<CreateReportByOperatorCommand, Result<Report>>
{
    public async Task<Result<Report>> Handle(CreateReportByOperatorCommand request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIDAsync(request.CategoryId);
        if (category is null)
            return NotFoundErrors.Category;
        
        var address = request.Address.GetAddress();
        //address.Location = new NetTopologySuite.Geometries.Point(request.Address.Longitude, request.Address.Latitude);
        var user = await userRepository.GetOrCreateCitizen(request.phoneNumber, request.firstName, request.lastName);

        List<Media> medias = new List<Media>();
        if (request.Attachments is not null)
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

        var report = Report.NewByOperator(
            request.operatorId,
            user.Id,
            user.PhoneNumber!,
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
        return report;
    }
}
