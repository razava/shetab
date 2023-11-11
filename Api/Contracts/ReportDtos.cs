using Api.Dtos;
using Application.Reports.Common;
using Domain.Models.Relational.Common;

namespace Api.Contracts;

public record CreateFeedbackDto(
    string token,
    int rating);

public record UpdateCommentDto(
    string Comment,
    bool? IsSeen,
    bool? IsVerified);

public record ReplyCommentDto(
    string Comment);

public record PutSatisfactionDto(
    int Rating,
    string Comment);


public record MakeTransitionDto(
    int TransitionId,
    int ReasonId,
    List<Guid> Attachments,
    string Comment,
    List<int> ActorIds)
{
    public List<Guid> Attachments { get; init; } = Attachments ?? new List<Guid>();
    public string Comment { get; init; } = Comment ?? "";
}


public record CitizenGetReportListDto(
    CategoryTitleDto Category,
    string Comments,
    AddresDetailDto Addres,
    string TrackingNumber,
    string LastStatus,
    DateTime Sent,
    IEnumerable<TransitionLogDto> TransitionLogs, //??
    bool IsLiked,
    int Likes,
    int CommentsCount,
    ICollection<MediaDto> Medias);

 

public record CitizenGetReportDetailsDto(
    ICollection<MediaDto> Medias,
    string LastStatus,
    CategoryDetailDto Category,
    string Comments,
    string TrackingNumber,
    DateTime Sent,
    AddresMoreDetailDto Address,
    int CommentsCount,
    int Likes,
    bool IsIdentityVisible);


public record CitizenCreateReportDto(
    int CategoryId,
    string Comments,
    AddressDto Address,
    List<Guid> Attachments,
    bool IsIdentityVisible);


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


//........................................
public class GetPossibleTransitionDto
{
    public string StageTitle { get; set; }
    public int TransitionId { get; set; }
    public IEnumerable<ReasonDto> ReasonList { get; set; }
    public IEnumerable<ActorDto> Actors { get; set; }
    public bool CanSendMessageToCitizen { get; set; }
    public TransitionType TransitionType { get; set; }
}

public class ReasonDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}
public class ActorDto
{
    public int Id { get; set; }
    public string Identifier { get; set; }
    public ActorType Type { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public string DisplayName { get { return Title + ((FirstName + LastName).Length > 0 ? " (" + FirstName + " " + LastName + ")" : ""); } }
    public string Organization { get; set; }
    public string PhoneNumber { get; set; }
    public List<ActorDto> Actors { get; set; }
}




public record MoveToStageDto(
    bool IsAccepted,
    int StageId,
    ICollection<ActorDto> Actors,
    string Comment,
    ICollection<Guid> Attachments,
    ActorType? ActorType,
    string ActorIdentifier,
    Visibility? Visibility);



public record GetPossibleSourceDto(
    string RoleId,
    string RoleName,
    string RoleTitle);


public record MessageToCitizenDto(
    List<Guid> Attachments,
    string Comment,
    bool IsPublic,
    string Message);



public class OperatorCreateReportDto
{
    public int CategoryId { get; set; }
    public string Comments { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsIdentityVisible { get; set; }
    public AddressDto Address { get; set; } = null!;
    public List<Guid> Attachments { get; set; } = new List<Guid>();

    //These are specific for verifying the report by operator
    public Guid? Id { get; set; }

    public ICollection<MediaDto> Medias { get; set; } = new List<MediaDto>();

    public Visibility Visibility { get; set; } = Visibility.Operators;
}



//public record OperatorCreateReportDto(
//    string PhoneNumber,
//    string FirstName,
//    string LastName,
//    int CategoryId,
//    string Comments,
//    AddressInfo Address,
//    List<Guid> Attachments,
//    bool IsIdentityVisible = true
//    //,visibility?
//    );

public class UpdateReportDto
{
    //public Guid Id { get; set; } 
    public int? CategoryId { get; set; }
    public string? Comments { get; set; }
    public bool? IsIdentityVisible { get; set; }
    public Visibility? Visibility { get; set; }
    public AddressDto? Address { get; set; }
    public List<Guid>? Attachments { get; set; }
}


public record ViolationPutDto(
    Guid Id,
    ViolationCheckResult? ViolationCheckResult,
    DateTime? ViolatoinCheckDateTime,
    string Comments);










