using DocumentFormat.OpenXml.Wordprocessing;
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
    public Priority? Priority { get; set; }
}


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
    string Comment);



