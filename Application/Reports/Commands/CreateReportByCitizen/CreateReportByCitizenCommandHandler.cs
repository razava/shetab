using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Messages;
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

        //TODO: Inform related users not all
        //await _hub.Clients.All.Update();
        
        return report;
    }
}
