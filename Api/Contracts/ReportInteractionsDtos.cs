﻿using Domain.Models.Relational;
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
    Guid ReportId,
    [Required]
    int ViolationTypeId,
    [MaxLength(512)]
    string Description);


public record CreateCommentViolationDto(
    [Required]
    Guid CommentId,
    [Required]
    int ViolationTypeId,
    [MaxLength(512)]
    string Description);



public record CreateCommentDto(
    [Required] [MaxLength(1024)]
    string Comment);


public record RestrictedUserDto(
    string FirstName,
    string LastName,
    string Title,
    string Organization,
    MediaDto Avatar);


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




//public record SatisfactionRatingDto(
//    int Rating);

