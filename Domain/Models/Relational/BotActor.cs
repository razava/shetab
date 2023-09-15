using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Relational;

public class BotActor
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }
    public int TransitionId { get; set; }
    public ProcessTransition Transition { get; set; } = null!;
    public ICollection<Actor> Actors { get; set; } = new List<Actor> ();
    public string MessageToCitizen { get; set; } = string.Empty;
    public Priority? Priority { get; set; }
    public Visibility? Visibility { get; set; }
    public int? ReasonId { get; set; }
    //consider category and state or transition history
    public ReasonMeaning? ReasonMeaning { get; set; }
}
