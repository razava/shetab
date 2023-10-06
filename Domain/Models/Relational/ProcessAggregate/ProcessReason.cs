using Domain.Models.Relational.Common;

namespace Domain.Models.Relational.ProcessAggregate;

public class ProcessReason
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ReasonMeaning ReasonMeaning { get; set; } = ReasonMeaning.Ok;

    public IEnumerable<ProcessTransition> Transitions { get; set; } = new List<ProcessTransition>();
}
