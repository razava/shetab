using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Mapster;
using MediatR;

namespace Application.Reports.Commands.AcceptByOperator;

internal sealed class AcceptByOperatorCommandHandler : IRequestHandler<AcceptByOperatorCommand, Report>
{
    private readonly IReportRepository _reportRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUploadRepository _uploadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptByOperatorCommandHandler(
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

    public async Task<Report> Handle(AcceptByOperatorCommand request, CancellationToken cancellationToken)
    {
        var report = await _reportRepository.GetByIDAsync(request.reportId);
        if (report == null)
            throw new Exception("Report not found");

        Category? category = null;
        if (request.CategoryId is not null)
        {
            category = await _categoryRepository.GetByIDAsync(request.CategoryId.Value);
            if (category is null)
            {
                //TODO: Handle this error
                throw new Exception();
            }

        }
        Address? address = null;
        if (request.Address is not null)
        {
            address = report.Address.Adapt<Address>();
            address.Location = new NetTopologySuite.Geometries.Point(request.Address.Longitude, request.Address.Latitude);
        }

        List<Media>? medias = null;
        if (request.Attachments is not null)
        {
            List<Upload> attachments = new List<Upload>();
            if (request.Attachments.Count > 0)
            {
                attachments = (await _uploadRepository
                .GetAsync(u => request.Attachments.Contains(u.Id) && u.UserId == request.operatorId))
                .ToList() ?? new List<Upload>();
                if (request.Attachments.Count != attachments.Count)
                {
                    throw new Exception("Attachments failure.");
                }
                attachments.ForEach(a => a.IsUsed = true);
                medias = attachments.Select(a => a.Media).ToList();
            }
        }

        report.Accept(
            request.operatorId,
            category,
            request.Comments,
            address,
            medias,
            null);

        await _unitOfWork.SaveAsync();

        return report;
    }
}
