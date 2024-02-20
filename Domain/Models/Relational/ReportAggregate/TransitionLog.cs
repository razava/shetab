using Domain.Messages;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;

namespace Domain.Models.Relational;

public class TransitionLog
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }
    public Guid ReportId { get; set; }
    public Report Report { get; set; } = null!;
    public ReportLogType ReportLogType { get; set; }
    public int? TransitionId { get; set; }
    public ProcessTransition? Transition { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public ICollection<Media> Attachments { get; set; } = new List<Media>();
    public int? ReasonId { get; set; }
    public ProcessReason? Reason { get; set; }
    public ActorType ActorType { get; set; }
    public string ActorIdentifier { get; set; } = null!;
    public double? Duration { get; set; }
    public bool IsPublic { get; set; }

    private TransitionLog() { }

    private TransitionLog(
        ReportLogType type,
        Guid reportId,
        int? transitionId,
        string? comment,
        List<Media>? attachments,
        string message,
        ActorType actorType,
        string actorIdentifier,
        int? reasonId,
        double? duration,
        bool isPublic)
    {
        if(attachments is not null)
            Attachments = attachments;
        ReportLogType = type;
        Comment = comment ?? "";
        DateTime = DateTime.UtcNow;
        ReportId = reportId;
        TransitionId = transitionId;
        Message = message;
        ActorType = actorType;
        ActorIdentifier = actorIdentifier;
        ReasonId = reasonId;
        Duration = duration;
        IsPublic = isPublic;
    }

    public static TransitionLog CreateTransition(
        Guid reportId,
        int transitionId,
        string? comment,
        List<Media>? attachments,
        string message,
        ActorType actorType,
        string actorIdentifier,
        int? reasonId,
        double duration,
        bool isPublic)
    {
        return new TransitionLog(
            ReportLogType.Transition,
            reportId,
            transitionId,
            comment,
            attachments,
            message,
            actorType,
            actorIdentifier,
            reasonId,
            duration,
            isPublic);
    }

    public static TransitionLog CreateMoveToStage(
        Guid reportId,
        string? comment,
        List<Media>? attachments,
        string message,
        ActorType actorType,
        string actorIdentifier,
        double duration,
        bool isPublic)
    {
        return new TransitionLog(
            ReportLogType.MoveToStage,
            reportId,
            null,
            comment,
            attachments,
            message,
            actorType,
            actorIdentifier,
            null,
            duration,
            isPublic);
    }

    public static TransitionLog CreateNewReport(
        Guid reportId,
        ActorType actorType,
        string actorIdentifier)
    {
        return new TransitionLog(
            ReportLogType.Created,
            reportId,
            null,
            null,
            null,
            ReportMessages.Created,
            actorType,
            actorIdentifier,
            null,
            null,
            true);
    }

    public static TransitionLog CreateApproved(
        Guid reportId,
        string actorIdentifier)
    {
        return new TransitionLog(
            ReportLogType.Created,
            reportId,
            null,
            null,
            null,
            ReportMessages.Created,
            ActorType.Person,
            actorIdentifier,
            null,
            null,
            true);
    }

    public static TransitionLog CreateMessageToCitizen(
            Guid reportId,
            string comment,
            List<Media>? attachments,
            string actorIdentifier,
            double duration)
    {
        return new TransitionLog(
            ReportLogType.MessageToCitizen,
            reportId,
            null,
            comment,
            attachments,
            ReportMessages.Responsed,
            ActorType.Person,
            actorIdentifier,
            null,
            duration,
            true);
    }

    public static TransitionLog CreateUpdate(
        Guid reportId,
        string comment,
        string actorIdentifier)
    {
        return new TransitionLog(
            ReportLogType.Change,
            reportId,
            null,
            comment,
            null,
            ReportMessages.Updated,
            ActorType.Person,
            actorIdentifier,
            null,
            null,
            true);
    }

    public static TransitionLog CreateFeedback(
        Guid reportId,
        string actorIdentifier,
        string comment)
    {
        return new TransitionLog(
            ReportLogType.Feedback,
            reportId,
            null,
            comment,
            null,
            ReportMessages.Feedbacked,
            ActorType.Person,
            actorIdentifier,
            null,
            null,
            true);
    }

    public static TransitionLog CreateObjection(
        Guid reportId,
        string? comment,
        List<Media>? attachments,
        string message,
        string userId)
    {
        return new TransitionLog(
            ReportLogType.Transition,
            reportId,
            null,
            comment,
            attachments,
            message,
            ActorType.Person,
            userId,
            null,
            null,
            true);
    }
}
