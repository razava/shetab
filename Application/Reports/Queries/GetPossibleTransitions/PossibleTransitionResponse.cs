using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;

namespace Application.Reports.Queries.GetPossibleTransitions;

public class PossibleTransitionResponse
{
    public string StageTitle { get; set; } = string.Empty;
    public int TransitionId { get; set; }
    public IEnumerable<ProcessReason> ReasonList { get; set; } = new List<ProcessReason>();
    public IEnumerable<Actor> Actors { get; set; } = new List<Actor>();
    public bool CanSendMessageToCitizen { get; set; }
    public TransitionType TransitionType { get; set; }
}

