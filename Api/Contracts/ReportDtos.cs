using Api.Dtos;
using Application.Reports.Common;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;


public record CitizenCreateReportDto(
    [Required]
    int CategoryId,
    [MaxLength(1024)]
    string Comments,
    AddressDto Address,
    List<Guid>? Attachments,
    bool IsIdentityVisible);


public record CitizenGetReportListDto(
    Guid Id,
    int CategoryId,
    CategoryTitleDto Category,//?
    GetCitizenShortDto Citizen,  //todo : ???? review this, depends on what application layer returns(display name?)
    string Comments,
    AddressDetailDto Address,
    string TrackingNumber,
    string LastStatus,
    DateTime Sent,
    IEnumerable<TransitionLogDto> TransitionLogs, //??
    bool IsLiked,
    int Likes,
    int CommentsCount,
    List<MediaDto> Medias);


public record GetCitizenShortDto(
    string FirstName,
    string LastName,
    MediaDto Avatar);


public record CitizenGetReportDetailsDto(
    List<MediaDto> Medias,
    string LastStatus,
    int CategoryId,
    CategoryDetailDto Category,//?
    string Comments,
    string TrackingNumber,
    DateTime Sent,
    AddressMoreDetailDto Address,
    double? Duration, //?
    double? ResponseDuration, //?
    DateTime Deadline,
    DateTime? ResponseDeadline,
    int CommentsCount,
    int Likes,
    bool IsIdentityVisible);


public class OperatorCreateReportDto
{
    [Required]
    public int CategoryId { get; set; }
    [MaxLength(1024)]
    public string Comments { get; set; } = null!;
    [Required] [MaxLength(16)]
    public string PhoneNumber { get; set; } = null!;
    [MaxLength(32)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(32)]
    public string LastName { get; set; } = string.Empty;
    public bool IsIdentityVisible { get; set; }
    public AddressDto Address { get; set; } = null!;
    public List<Guid> Attachments { get; set; } = new List<Guid>();

    //These are specific for verifying the report by operator
    public Guid? Id { get; set; }

    //public ICollection<MediaDto> Medias { get; set; } = new List<MediaDto>();

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
    [MaxLength(1024)]
    public string? Comments { get; set; }
    public bool? IsIdentityVisible { get; set; }
    public Visibility? Visibility { get; set; }
    public AddressDto? Address { get; set; }
    public List<Guid>? Attachments { get; set; }
}


public record StaffGetReportListDto(
    Guid Id,
    string LastStatus,
    string TrackingNumber,
    int CategoryId,
    DateTime Sent,
    DateTime Deadline,
    DateTime? ResponseDeadline,
    int? Rating
    //int Rating
    );     //todo : Satisfaction.rating is correct


public record StaffGetReportDetailsDto(
    Guid Id,
    string TrackingNumber,
    string LastStatus,
    int CategoryId,
    GetShortCategoryDto Category,//?
    string CitizenId,
    AddressReportGet Address,
    string Comments,
    DateTime Sent,
    DateTime Deadline,
    DateTime? ResponseDeadline,
    int? Rating,
    Visibility Visibility,
    bool IsIdentityVisible,
    int Likes,
    int CommentsCount,
    List<MediaDto> Medias
    //Satisfaction [rating, comments, id]
    );


public record FilterGetReports(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<ReportState>? CurrentStates,
    [MaxLength(64)]
    string? Query);


public record FilterGetAllReports(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    //todo : annotation for strings?
    List<string>? RoleNames,
    List<int>? CategoryIds,
    List<int>? RegionIds,
    List<ReportState>? CurrentStates,
    bool HasSatisfaction,  //??
    int MinSatisfaction,   //??
    int MaxSatisfaction,   //??
    [MaxLength(64)]
    string? Query);



public record MakeTransitionDto(
    [Required]
    int TransitionId,
    [Required]
    int ReasonId,
    List<Guid> Attachments,
    [MaxLength(512)]
    string Comment,
    [Required]
    int ToActorId)
{
    public List<Guid> Attachments { get; init; } = Attachments ?? new List<Guid>();
    public string Comment { get; init; } = Comment ?? "";
}


public class TransitionLogDto
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public List<MediaDto> Attachments { get; set; } = new List<MediaDto>();
    public ReasonDto Reason { get; set; } = default!;
    public ActorType ActorType { get; set; }
    public string ActorIdentifier { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public ActorShortDto Actor { get; set; } = default!;   //todo : Handle from Application Layer
}

public class ActorShortDto
{
    //todo : is this correct that set ? for fix warnings here?
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Title { get; set; }
    public string DisplayName { get { return Title + ((FirstName + LastName).Length > 0 ? " (" + FirstName + " " + LastName + ")" : ""); } }
}


public class GetPossibleTransitionDto
{
    public string StageTitle { get; set; } = string.Empty;
    public int TransitionId { get; set; }
    public IEnumerable<ReasonDto> ReasonList { get; set; } = new List<ReasonDto>();
    public IEnumerable<ActorDto> Actors { get; set; } = new List<ActorDto>();
    public bool CanSendMessageToCitizen { get; set; }
    public TransitionType TransitionType { get; set; }
}

public class ReasonDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
public class ActorDto
{
    public int Id { get; set; }
    public string Identifier { get; set; } = string.Empty;
    public ActorType Type { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string DisplayName { get { return Title + ((FirstName + LastName).Length > 0 ? " (" + FirstName + " " + LastName + ")" : ""); } }
    public string Organization { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<ActorDto> Actors { get; set; } = new List<ActorDto>();
}

//todo : review annotations
public record InspectorTransitionDto(
    [Required]
    bool IsAccepted,
    List<Guid> Attachments,
    [Required][MaxLength(512)]
    string Comment,
    [Required]
    int ToActorId,
    [Required]
    int StageId,
    Visibility? Visibility);


public record GetPossibleSourceDto(
    string RoleId,
    string RoleName,
    string RoleTitle);


//todo : review annotations
public record MessageToCitizenDto(
    List<Guid> Attachments,
    [MaxLength(512)]
    string Comment,
    bool IsPublic,
    [MaxLength(512)]
    string Message);



