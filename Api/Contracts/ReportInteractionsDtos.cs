using Domain.Models.Relational;
using Domain.Models.Relational.Common;

namespace Api.Contracts;

public record CreateFeedbackDto(
    string token,
    int rating);

public record UpdateCommentDto(
    string Comment,
    bool? IsSeen,         //..... ??
    bool? IsVerified);    //..... ??

public record ReplyCommentDto(
    string Comment);

public record PutSatisfactionDto(
    int Rating,
    string Comment);

public record CreateReportViolationDto(
    Guid ReportId,
    int ViolationTypeId,
    string Description);


public record CreateCommentViolationDto(
    Guid CommentId,
    int ViolationTypeId,
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
    string Comments);


public record FilterGetCommentViolation(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<int>? CategoryIds,
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

