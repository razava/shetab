namespace Domain.Models.Relational.ProcessAggregate;

public class Process : BaseModel
{
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int Id { get; set; }
    public ICollection<ProcessStage> Stages { get; set; } = new List<ProcessStage>();
    public ICollection<ProcessTransition> Transitions { get; set; } = new List<ProcessTransition>();
    public RevisionUnit? RevisionUnit { get; set; }
    public bool IsDeleted { get; set; } = false;
}


