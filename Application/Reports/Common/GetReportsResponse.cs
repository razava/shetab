namespace Application.Reports.Common;

public record GetReportsResponse(
    Guid Id,
    string LastStatus,
    string TrackingNumber,
    int CategoryId,
    string CategoryTitle,
    DateTime Sent,
    DateTime Deadline,
    DateTime? ResponseDeadline,
    int? Rating
    );