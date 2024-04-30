using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;


public record CitizenCreateReportDto(
    [Required]
    int instanceId,
    [Required]
    int CategoryId,
    [MaxLength(1024)]
    string Comments,
    AddressDto Address,
    List<Guid>? Attachments,
    bool IsIdentityVisible);


public record CitizenObjectReportDto(
    [MaxLength(1024)]
    string Comments,
    List<Guid>? Attachments);



public class OperatorCreateReportDto
{
    [Required]
    public int CategoryId { get; set; }
    [MaxLength(1024)]
    public string Comments { get; set; } = null!;
    [Required] [MaxLength(16)]
    public string PhoneNumber { get; set; } = null!;
    [MaxLength(64)]
    public string FirstName { get; set; } = string.Empty;
    [MaxLength(64)]
    public string LastName { get; set; } = string.Empty;
    public bool IsIdentityVisible { get; set; }
    public AddressDto Address { get; set; } = null!;
    public List<Guid> Attachments { get; set; } = new List<Guid>();

    //These are specific for verifying the report by operator
    public Guid? Id { get; set; }

    //public ICollection<MediaDto> Medias { get; set; } = new List<MediaDto>();

    public Visibility Visibility { get; set; } = Visibility.Operators;
    public Priority Priority { get; set; } = Priority.Normal;
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


public record MessageToCitizenDto(
    List<Guid> Attachments,
    [MaxLength(512)]
    string Comment);



