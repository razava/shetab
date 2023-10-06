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
    public ICollection<Guid> Attachments { get; set; } = new List<Guid>();
    public int? ReasonId { get; set; }
    public ProcessReason? Reason { get; set; }
    public ActorType ActorType { get; set; }
    public string ActorIdentifier { get; set; } = null!;
    public double? Duration { get; set; }
    public bool IsPublic { get; set; }
}
