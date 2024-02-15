using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate.Events;

namespace Application.Complaints.Events;

internal sealed class ReportCreatedDomainEventHandler(
    IUserRepository userRepository,
    ICommunicationService communicationService) : INotificationHandler<ReportCreatedDomainEvent>
{
    public async Task Handle(ReportCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        
    }
}
