using Domain.Models.Relational;

namespace Api.Dtos;

public class ViolationCreateDto
{
    public Guid Id { get; set; }
    public Guid? ReportId { get; set; }
    public Guid? CommentId { get; set; }
    public string Description { get; set; }
    public int ViolationTypeId { get; set; }
}

public class ViolationGetDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUserDto User { get; set; }
    public Guid? ReportId { get; set; }
    public AdminGetReportDto Report { get; set; }
    public Guid? CommentId { get; set; }
    public GetCommentForOperatorDto Comment { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public int ViolationTypeId { get; set; }
    public ViolationType ViolationType { get; set; }
    public ViolationCheckResult? ViolationCheckResult { get; set; }
    public DateTime? ViolatoinCheckDateTime { get; set; }
}

public class ViolationPutDto
{
    public Guid Id { get; set; }
    public ViolationCheckResult? ViolationCheckResult { get; set; }
    public DateTime? ViolatoinCheckDateTime { get; set; }
    public string Comments { get; set; }
}
