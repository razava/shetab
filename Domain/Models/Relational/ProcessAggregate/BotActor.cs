using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models.Relational.Common;

namespace Domain.Models.Relational.ProcessAggregate;

public class BotActor
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = null!;
    public int TransitionId { get; set; }
    public ProcessTransition Transition { get; set; } = null!;
    public int DestinationActorId { get; set; }
    public Actor DestinationActor { get; set; } = null!;
    public Actor Actor { get; set; } = null!;
    public string MessageToCitizen { get; set; } = string.Empty;
    public Priority? Priority { get; set; }
    public Visibility? Visibility { get; set; }
    public int? ReasonId { get; set; }
    //consider category and state or transition history
    public ReasonMeaning? ReasonMeaning { get; set; }
}
