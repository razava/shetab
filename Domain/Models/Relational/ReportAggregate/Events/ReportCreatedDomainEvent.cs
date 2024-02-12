﻿using Domain.Primitives;

namespace Domain.Models.ComplaintAggregate.Events;

public record ReportCreatedDomainEvent(
    Guid Id,
    Guid ComplaintId,
    string TrackingNumber,
    string Password,
    string? UserId) : DomainEvent(Id);

