namespace Domain.Models.Relational;

public class Violation : BaseModel
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public Guid? ReportId { get; set; }
    public Report? Report { get; set; }
    public Guid? CommentId { get; set; }
    public Comment? Comment { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public int ViolationTypeId { get; set; }
    public ViolationType ViolationType { get; set; } = null!;
    public ViolationCheckResult? ViolationCheckResult { get; set; }
    public DateTime? ViolatoinCheckDateTime { get; set; }
}


