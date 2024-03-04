using Application.Common.Exceptions;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate.Events;
using Domain.Models.Relational;
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
        string? phoneNumber = null;
        List<string> userIds = new List<string>();
        var reportId = notification.Report.Id;
        var report = notification.Report;
        switch (notification.EventType)
        {
            case ReportDomainEventTypes.CreatedByCitizen:
                message = "درخواست جدیدی در سامانه ثبت شد.";
                userIds.AddRange(await getOperatorIdentifiers(report));
                break;
            case ReportDomainEventTypes.Updated:
                message = "اطلاعات یک درخواست بروزرسانی شد.";
                userIds.AddRange(await getOperatorIdentifiers(report));
                break;
            case ReportDomainEventTypes.Accepted:
                break;
            case ReportDomainEventTypes.Refered:
                message = "یک درخواست به شما ارجاع داده شد.";
                var identifier = await getUserIdentifier(report);
                userIds.Add(identifier);
                phoneNumber = await getUserPhoneNumber(identifier);
                break;
            case ReportDomainEventTypes.Finished:
                message = "درخواست شما رسیدگی شد.";
                userIds.Add(report.CitizenId);
                phoneNumber = await getUserPhoneNumber(report.CitizenId);
                break;
            case ReportDomainEventTypes.Responsed:
                message = "واحد اجرایی به درخواست شما پاسخ داد.";
                userIds.Add(report.CitizenId);
                phoneNumber = await getUserPhoneNumber(report.CitizenId);
                break;
            case ReportDomainEventTypes.CreatedByOperator:
                break;

        }
        if (userIds.Any())
        {
            await communicationService.SendNotification(userIds, method, message, reportId);
        }
        if(phoneNumber is not null)
        {
            await communicationService.SendAsync(phoneNumber, message);
        }
    }

    private async Task<string> getUserIdentifier(Report report)
    {
        return (await unitOfWork.DbContext.Set<Actor>()
                            .Where(a => a.Id == report.CurrentActorId)
                            .Select(a => a.Identifier)
                            .FirstOrDefaultAsync()) ?? "";
    }

    private async Task<string?> getUserPhoneNumber(string id)
    {
        return (await unitOfWork.DbContext.Set<ApplicationUser>()
                            .Where(u => u.Id == id && u.SmsAlert)
                            .Select(a => a.PhoneNumber)
                            .FirstOrDefaultAsync());
    }

    private async Task<List<string>> getOperatorIdentifiers(Report report)
    {
        return await unitOfWork.DbContext.Set<OperatorCategory>()
                                .Where(oc => oc.CategoryId == report.CategoryId)
                                .Select(oc => oc.OperatorId)
                                .ToListAsync();
    }
}
