using Domain.Primitives;

namespace Domain.Models.ComplaintAggregate.Events;

public record ReportCreatedByOperatorDomainEvent(
    Guid Id,
    int InstanceId,
    Guid ReportId,
    DateTime DateTime,
    string TrackingNumber,
    string CitizenId) : DomainEvent(Id);

public record ReportCreatedByCitizenDomainEvent(
    Guid Id,
    int InstanceId,
    Guid ReportId,
    DateTime DateTime,
    string TrackingNumber,
    string CitizenId) : DomainEvent(Id);

public record ReportResponsedDomainEvent(
    Guid Id,
    int InstanceId,
    Guid ReportId,
    DateTime DateTime,
    string TrackingNumber,
    string CitizenId) : DomainEvent(Id);

public record ReportAcceptedDomainEvent(
    Guid Id,
    int InstanceId,
    Guid ReportId,
    DateTime DateTime,
    string TrackingNumber,
    string CitizenId) : DomainEvent(Id);

public record ReportUpdatedDomainEvent(
    Guid Id,
    int InstanceId,
    Guid ReportId,
    DateTime DateTime,
    string TrackingNumber,
    string CitizenId) : DomainEvent(Id);

public record ReportReferedDomainEvent(
    Guid Id,
    int InstanceId,
    Guid ReportId,
    DateTime DateTime,
    string TrackingNumber,
    string CitizenId) : DomainEvent(Id);

