using Domain.Primitives;

namespace Domain.Models.ComplaintAggregate.Events;

public record ReportUpdatedDomainEvent(
    Guid Id,
    Guid ComplaintId,
    string TrackingNumber,
    string? UserId) : DomainEvent(Id);

