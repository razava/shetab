using Domain.Models.Relational.Common;

namespace Application.Reports.Queries.GetPossibleTransitions;

public class PossibleTransitionDto
{
    public string StageTitle { get; set; }
    public int TransitionId { get; set; }
    public IEnumerable<ReasonDto> ReasonList { get; set; }
    public IEnumerable<ActorDto> Actors { get; set; }
    public bool CanSendMessageToCitizen { get; set; }
    public TransitionType TransitionType { get; set; }
}

public class ReasonDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
public class ActorDto
{
    public int Id { get; set; }
    public string Identifier { get; set; }
    public ActorType Type { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public string DisplayName { get { return Title + ((FirstName + LastName).Length > 0 ? " (" + FirstName + " " + LastName + ")" : ""); } }
    public string Organization { get; set; }
    public string PhoneNumber { get; set; }
    public List<ActorDto> Actors { get; set; }
}
