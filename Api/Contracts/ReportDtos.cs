using Api.Dtos;
using Domain.Models.Relational.Common;

namespace Api.Contracts;

public record CreateFeedbackDto(
    string token,
    int rating);

public record UpdateCommentDto(
    Guid ReportId,   /*??*/
    string Comment,
    bool? IsSeen,
    bool? IsVerified);

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


public record CreateViolationDto(
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








