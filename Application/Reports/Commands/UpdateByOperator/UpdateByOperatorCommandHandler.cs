using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Mapster;
using MediatR;

namespace Application.Reports.Commands.UpdateByOperator;

internal sealed class UpdateByOperatorCommandHandler : IRequestHandler<UpdateByOperatorCommand, Report>
{
    private readonly IReportRepository _reportRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateByOperatorCommandHandler(IUnitOfWork unitOfWork, IReportRepository reportRepository, ICategoryRepository categoryRepository)
    {
        _unitOfWork = unitOfWork;
        _reportRepository = reportRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Report> Handle(UpdateByOperatorCommand request, CancellationToken cancellationToken)
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
            address = request.Address.Adapt<Address>();
        }

        report.Update(
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
