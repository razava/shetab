using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Mapster;
using MediatR;

namespace Application.Reports.Commands.CreateReportByCitizen;

internal sealed class CreateReportByCitizenCommandHandler : IRequestHandler<CreateReportByCitizenCommand, Report>
{
    private readonly IReportRepository _reportRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReportByCitizenCommandHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Report> Handle(CreateReportByCitizenCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIDAsync(request.CategoryId);
        if(category is null)
        {
            //TODO: Handle this error
            throw new Exception();
        }
        var address = request.Address.Adapt<Address>();
        var report = Report.NewByCitizen(
            request.citizenId,
            request.phoneNumber,
            category,
            request.Comments,
            address,
            request.Attachments,
            Visibility.EveryOne,
            Priority.Normal,
            request.IsIdentityVisible);

        _reportRepository.Insert(report);
        await _unitOfWork.SaveAsync();

        /*
        await CommunicationServices.AddNotification(
            new Message()
            {
                ShahrbinInstanceId = report.ShahrbinInstanceId,
                Title = "ثبت درخواست" + " - " + report.TrackingNumber,
                Content = ReportMessages.Created,
                DateTime = report.Sent,
                MessageType = MessageType.Report,
                SubjectId = report.Id,
                Recepients = new List<MessageRecepient>()
                {
                    new MessageRecepient() { Type = RecepientType.Person, ToId = report.CitizenId }
                }
            },
            _context);
        await _context.SaveChangesAsync();

        //TODO: Inform related users not all
        await _hub.Clients.All.Update();
        */
        return report;
    }
}
