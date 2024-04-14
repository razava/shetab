using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Setup.Commands.FormatReportComments;

internal class FormatReportCommentsCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<FormatReportCommentsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(FormatReportCommentsCommand request, CancellationToken cancellationToken)
    {
        //remove invalid reports (old reports that heve forms with old structure for store)
        var invalidReports = await unitOfWork.DbContext.Set<Report>()
            .Where(r => !r.IsDeleted && r.Comments.StartsWith("{") && r.ShahrbinInstanceId == request.instanceId)
            .ToListAsync();
        foreach (var item in invalidReports)
        {
            item.Delete();
        }
        unitOfWork.DbContext.Set<Report>().AttachRange(invalidReports);
        await unitOfWork.SaveAsync();

        var defaultFormId =await unitOfWork.DbContext.Set<Form>()
            .AsNoTracking().Where(f => f.Title == "default" && f.ShahrbinInstanceId == request.instanceId)
            .Select(f => f.Id).FirstOrDefaultAsync();


        if (defaultFormId == default(Guid)) throw new Exception("Dafault form not found");

        //convert structure of old reports that were without form
        var reports = await unitOfWork.DbContext.Set<Report>()
            .Where(r => !r.IsDeleted && !r.Comments.StartsWith("{") && r.ShahrbinInstanceId == request.instanceId)
            .ToListAsync();

        foreach (var report in reports)
        {
            //var comments = $"{{\"values\":[{{\"id\":1,\"name\":\"توضیحات\",\"value\":\"{report.Comments}\"}}],\"formId\":\"417561fc-6736-4ce0-a87c-620b0b876a94\"}}";
            var comments = $"{{\"values\":[{{\"id\":1,\"name\":\"توضیحات\",\"value\":\"{report.Comments}\"}}],\"formId\":\"" + defaultFormId.ToString() + "\"}";

            report.UpdateComments(comments);
        }

        unitOfWork.DbContext.Set<Report>().AttachRange(reports);
        await unitOfWork.SaveAsync();
        return true;

    }


}
