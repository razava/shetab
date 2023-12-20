﻿using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Reports.Commands.CreateReportByCitizen;

internal sealed class CreateReportByCitizenCommandHandler : IRequestHandler<CreateReportByCitizenCommand, Report>
{
    private readonly IReportRepository _reportRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUploadRepository _uploadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReportByCitizenCommandHandler(
        IUnitOfWork unitOfWork,
        IReportRepository reportRepository,
        ICategoryRepository categoryRepository,
        IUploadRepository uploadRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _categoryRepository = categoryRepository;
        _uploadRepository = uploadRepository;
    }

    public async Task<Report> Handle(CreateReportByCitizenCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIDAsync(request.CategoryId);
        if(category is null)
        {
            //TODO: Handle this error
            throw new NotFoundException("Category");
        }
        var address = request.Address.GetAddress();
        //address.Location = new NetTopologySuite.Geometries.Point(request.Address.Longitude, request.Address.Latitude);

        List<Media> medias = new List<Media>();
        if (request.Attachments is not null)
        {
            List<Upload> attachments = new List<Upload>();
            if (request.Attachments.Count > 0)
            {
                attachments = (await _uploadRepository
                .GetAsync(u => request.Attachments.Contains(u.Id) && u.UserId == request.citizenId))
                .ToList() ?? new List<Upload>();
                if (request.Attachments.Count != attachments.Count)
                {
                    throw new AttachmentsFailureException();
                }
                attachments.ForEach(a => a.IsUsed = true);
                medias = attachments.Select(a => a.Media).ToList();
            }
        }

        var report = Report.NewByCitizen(
            request.citizenId,
            request.phoneNumber,
            category,
            request.Comments,
            address,
            medias,
            Visibility.EveryOne,
            Priority.Normal,
            request.IsIdentityVisible);

        _reportRepository.Insert(report);
        await _unitOfWork.SaveAsync();

        //TODO: Inform related users not all
        //await _hub.Clients.All.Update();
        
        return report;
    }
}
