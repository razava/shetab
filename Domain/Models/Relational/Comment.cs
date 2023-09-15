namespace Domain.Models.Relational;

public class Comment : BaseModel
{
    public Guid Id { get; set; }
    public string Text { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public bool IsVerified { get; set; }
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public Guid? ReportId { get; set; }
    public Report? Report { get; set; }
    public bool IsSeen { get; set; }
    public Guid? ReplyId { get; set; }
    public Comment? Reply { get; set; }
    public ICollection<Violation> Violations { get; set; } = new List<Violation>();
}
