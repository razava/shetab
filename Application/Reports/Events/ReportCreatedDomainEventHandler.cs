using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Messages;
using Domain.Models.ComplaintAggregate.Events;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ReportAggregate;

namespace Application.Complaints.Events;

internal sealed class ReportCreatedDomainEventHandler(
    IMessageRepository messageRepository,
    IUserRepository userRepository,
    ICommunicationService communicationService) : INotificationHandler<ReportCreatedByOperatorDomainEvent>
{
    public async Task Handle(ReportCreatedByOperatorDomainEvent notification, CancellationToken cancellationToken)
    {
        var message = new Message()
        {
            ShahrbinInstanceId = notification.InstanceId,
            Title = "ثبت درخواست" + " - " + notification.TrackingNumber,
            Content = ReportMessages.Created,
            DateTime = notification.DateTime,
            MessageType = MessageType.Report,
            SubjectId = notification.ReportId,
            Recepient = MessageRecepient.Create(RecepientType.Person, notification.CitizenId)
        };
        messageRepository.Insert(message);
    }
}
