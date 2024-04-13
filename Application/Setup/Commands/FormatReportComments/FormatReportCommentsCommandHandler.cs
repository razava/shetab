using Application.Common.Interfaces.Persistence;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;

namespace Application.Setup.Commands.FormatReportComments;

internal class FormatReportCommentsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<FormatReportCommentsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(FormatReportCommentsCommand request, CancellationToken cancellationToken)
    {
        var reports = await unitOfWork.DbContext.Set<Report>()
            .Where(r => !r.IsDeleted && !r.Comments.StartsWith('{'))
        .ToListAsync();
        //foreach (var report in reports)
        //{
        //    report.Comments = $"{\"values\":[{\"id\":1,\"name\":\"توضیحات\",\"value\":{}}],\"formId\":\"417561fc - 6736 - 4ce0 - a87c - 620b0b876a94\"}";

        //    report.Comments = $"kkk {report.Comments}";

        //}
        return true;

    }
}
