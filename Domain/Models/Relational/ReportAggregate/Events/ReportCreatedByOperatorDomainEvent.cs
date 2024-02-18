using Domain.Models.Relational;
using Domain.Primitives;

namespace Domain.Models.ComplaintAggregate.Events;

public enum ReportDomainEventTypes
{
    CreatedByCitizen,
    CreatedByOperator,
    Accepted,
    Responsed,
    Updated,
    Refered,
    Finished
}

public record ReportDomainEvent(
    Guid Id,
    ReportDomainEventTypes EventType,
    Report Report) : DomainEvent(Id);

