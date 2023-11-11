using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;

namespace Application.Reports.Queries.GetPossibleTransitions;

public class PossibleTransitionResponse
{
    public string StageTitle { get; set; } = string.Empty;
    public int TransitionId { get; set; }
    public IEnumerable<ProcessReason> ReasonList { get; set; } = new List<ProcessReason>();
    public IEnumerable<ActorWithName> Actors { get; set; } = new List<ActorWithName>();
    public bool CanSendMessageToCitizen { get; set; }
    public TransitionType TransitionType { get; set; }
}


public class ActorWithName
{
    public int Id { get; set; }
    public string Identifier { get; set; } = null!;
    public ActorType Type { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string DisplayName 
    { 
        get { return Title + ((FirstName + LastName).Length > 0 ? " (" + FirstName + " " + LastName + ")" : ""); } 
    }
    public string Organization { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
