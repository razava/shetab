using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate.Events;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Complaints.Events;

internal sealed class ReportDomainEventHandler(
    IUnitOfWork unitOfWork,
    ICommunicationService communicationService) : INotificationHandler<ReportDomainEvent>
{
    public async Task Handle(ReportDomainEvent notification, CancellationToken cancellationToken)
    {
        var method = "UPDATE";
        var message = "";
        List<string> userIds = new List<string>();
        var reportId = notification.Report.Id;
        var report = notification.Report;
        switch (notification.EventType)
        {
            case ReportDomainEventTypes.CreatedByCitizen:
                message = "درخواست جدیدی در سامانه ثبت شد.";
                userIds.AddRange(
                    await unitOfWork.DbContext.Set<OperatorCategory>()
                        .Where(oc => oc.CategoryId == report.CategoryId)
                        .Select(oc => oc.OperatorId)
                        .ToListAsync());
                break;
            case ReportDomainEventTypes.Refered:
                message = "یک درخواست به شما ارجاع داده شد.";
                userIds.Add(await unitOfWork.DbContext.Set<Actor>()
                    .Where(a => a.Id == report.CurrentActorId)
                    .Select(a => a.Identifier)
                    .FirstOrDefaultAsync() ?? "");
                break;
            case ReportDomainEventTypes.Updated:
                message = "اطلاعات یک درخواست بروزرسانی شد.";
                userIds.AddRange(
                    await unitOfWork.DbContext.Set<OperatorCategory>()
                        .Where(oc => oc.CategoryId == report.CategoryId)
                        .Select(oc => oc.OperatorId)
                        .ToListAsync());
                break;
            case ReportDomainEventTypes.Finished:
                message = "درخواست شما رسیدگی شد.";
                userIds.Add(report.CitizenId);
                break;
            case ReportDomainEventTypes.Responsed:
                message = "واحد اجرایی به درخواست شما پاسخ داد.";
                userIds.Add(report.CitizenId);
                break;
            case ReportDomainEventTypes.Accepted: 
                break;
            case ReportDomainEventTypes.CreatedByOperator:
                break;

        }
        if (userIds.Any())
        {
            await communicationService.SendNotification(userIds, method, message, reportId);
        }        
    }
}
