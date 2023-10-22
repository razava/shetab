using Domain.Models.Relational.Common;

namespace Domain.Models.Relational.ProcessAggregate;

public class ProcessTransition
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Message { get; set; } = null!;
    public int FromId { get; set; }
    public ProcessStage From { get; set; } = null!;
    public int ToId { get; set; }
    public ProcessStage To { get; set; } = null!;
    public ReportState ReportState { get; set; }

    public IEnumerable<ProcessReason> ReasonList { get; set; } = new List<ProcessReason>();

    public bool CanSendMessageToCitizen { get; set; }
    public string? Routine { get; set; }
    public TransitionType TransitionType { get; set; }
    public int Order { get; set; }
    public bool IsTransitionLogPublic { get; set; }
    public string Status { get; set; } = string.Empty;
}
