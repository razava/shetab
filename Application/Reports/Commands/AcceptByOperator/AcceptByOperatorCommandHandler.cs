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
    private readonly IUnitOfWork _unitOfWork;

    public AcceptByOperatorCommandHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _categoryRepository = categoryRepository;
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
        }

        report.Accept(
            request.operatorId,
            category,
            request.Comments,
            address,
            request.Attachments,
            null);

        await _unitOfWork.SaveAsync();

        return report;
    }
}
