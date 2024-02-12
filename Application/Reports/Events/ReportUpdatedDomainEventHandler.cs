using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Domain.Models.ComplaintAggregate.Events;

namespace Application.Complaints.Events;

internal sealed class ReportUpdatedDomainEventHandler(
    IUserRepository userRepository,
    ICommunicationService communicationService) : INotificationHandler<ReportUpdatedDomainEvent>
{
    public async Task Handle(ReportUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        
    }
}
