using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Messages;
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
    private readonly IUnitOfWork _unitOfWork;

    public CreateReportByOperatorCommandHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository, ICategoryRepository categoryRepository, IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
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
        var user = await _userRepository.GetOrCreateCitizen(request.phoneNumber, request.firstName, request.lastName);

        var report = Report.NewByOperator(
            request.operatorId,
            user.Id,
            user.PhoneNumber!,
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
