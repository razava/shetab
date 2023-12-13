﻿using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;

public record CreateFeedbackDto(
    [MaxLength(1024)]
    string token,
    [Required]
    int rating);

public record UpdateCommentDto(
    [Required] [MaxLength(512)]
    string Comment,
    bool? IsSeen,         //..... ??
    bool? IsVerified);    //..... ??

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


public record CitizenGetComments(
    Guid Id,
    string Text,
    DateTime DateTime,
    RestrictedUserDto User,
    Guid ReportId,
    CitizenGetComments Reply,
    bool CanDelete);

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


public record FilterGetCommentViolation(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<int>? CategoryIds,
    [MaxLength(64)]
    string? Query);


public record GetCommentsDto(
    GetShortUserDto User,
    string Text);


public record GetShortUserDto(
    string FirstName,
    string LastName,
    string UserName,
    MediaDto Avatar);


public record GetViolationsDto(
    Guid? CommentId,
    Guid? ReportId,
    ViolationTypeDto ViolationType,
    DateTime DateTime,
    string Discription);

//todo : fix this later
public class ViolationTypeDto : ViolationType { }


//public record SatisfactionRatingDto(
//    int Rating);

