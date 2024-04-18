using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;

public record CreateFeedbackDto(
    [Required]
    int Rating);

public record UpdateCommentDto(
    [Required] [MaxLength(512)]
    string Comment,
    bool? IsSeen,    
    bool? IsVerified);

public record ReplyCommentDto(
    [Required] [MaxLength(512)]
    string Comment);

public record PutSatisfactionDto(
    [Required]
    int Rating,
    [Required] [MaxLength(512)]
    string Comment);

public record CreateReportViolationDto(
    [Required]
    int instanceId,
    [Required]
    int ViolationTypeId,
    [MaxLength(512)]
    string Description);


public record CreateCommentViolationDto(
    [Required]
    int instanceId,
    [Required]
    int ViolationTypeId,
    [MaxLength(512)]
    string Description);



public record CreateCommentDto(
    [Required] [MaxLength(1024)]
    string Comment);



public record ViolationPutDto(
    Guid Id,
    ViolationCheckResult? ViolationCheckResult,
    DateTime? ViolatoinCheckDateTime,
    [MaxLength(512)]
    string Comments);



public record GetViolationsDto(
    Guid? CommentId,
    Guid? ReportId,
    ViolationType ViolationType,
    DateTime DateTime,
    string Discription);

