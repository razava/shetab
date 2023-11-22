using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Mapster;
using MediatR;

namespace Application.Reports.Commands.CreateReportByOperator;

internal sealed class CreateReportByOperatorCommandHandler : IRequestHandler<CreateReportByOperatorCommand, Report>
{
    private readonly IReportRepository _reportRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUploadRepository _uploadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReportByOperatorCommandHandler(
        IUnitOfWork unitOfWork,
        IReportRepository reportRepository,
        ICategoryRepository categoryRepository,
        IUserRepository userRepository,
        IUploadRepository uploadRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
        _uploadRepository = uploadRepository;
    }

    public async Task<Report> Handle(CreateReportByOperatorCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIDAsync(request.CategoryId);
        if (category is null)
        {
            //TODO: Handle this error
            throw new Exception();
        }
        var address = request.Address.Adapt<Address>();
        //address.Location = new NetTopologySuite.Geometries.Point(request.Address.Longitude, request.Address.Latitude);
        var user = await _userRepository.GetOrCreateCitizen(request.phoneNumber, request.firstName, request.lastName);

        List<Media> medias = new List<Media>();
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

        _reportRepository.Insert(report);
        await _unitOfWork.SaveAsync();


        //TODO: Inform related users not all
        //await _hub.Clients.All.Update();
        return report;
    }
}
