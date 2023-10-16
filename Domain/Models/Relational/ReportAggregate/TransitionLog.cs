using Amazon.Auth.AccessControlPolicy;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;
using MongoDB.Driver.Core.Operations;
using System.Net.Mail;

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
        Guid reportId,
        int? transitionId,
        string? comment,
        List<Guid>? attachments,
        string message,
        ActorType actorType,
        string actorIdentifier,
        int? reasonId,
        TimeSpan? duration,
        bool isPublic)
    {
        if(attachments is not null)
            attachments.ForEach(p => Attachments.Add(new Media() { Id = p }));
        Comment = comment ?? "";
        DateTime = DateTime.UtcNow;
        ReportId = reportId;
        TransitionId = transitionId;
        Message = message;
        ActorType = actorType;
        ActorIdentifier = actorIdentifier;
        ReasonId = reasonId;
        Duration = duration?.TotalSeconds;
        IsPublic = isPublic;
    }

    public static TransitionLog Create(
        Guid reportId,
        int? transitionId,
        string? comment,
        List<Guid>? attachments,
        string message,
        ActorType actorType,
        string actorIdentifier,
        int? reasonId,
        TimeSpan? duration,
        bool isPublic)
    {
        return new TransitionLog(reportId, transitionId, comment, attachments, message, actorType, actorIdentifier, reasonId, duration, isPublic);
    }
}
