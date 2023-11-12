using Api.Contracts;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;
using System.Runtime.CompilerServices;

namespace Api.Dtos;

public class CategoryDto
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string AttachmentDescription { get; set; }
    public int Duration { get; set; }
    public int? ResponseDuration { get; set; }
    public List<FormElement> FormElements { get; set; } = new List<FormElement>();
    public List<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    public bool ObjectionAllowed { get; set; }
    public bool HideMap { get; set; }
}

public class CategoryCreateDto
{
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public int? ProcessId { get; set; }
    public string Description { get; set; }
    public string AttachmentDescription { get; set; }
    public int Duration { get; set; }
    public int? ResponseDuration { get; set; }
    //public ICollection<FormElement> FormElements { get; set; } = new List<FormElement>();
    public bool ObjectionAllowed { get; set; }
    //public bool EditingAllowed { get; set; } = true;
    public bool HideMap { get; set; }
}

public class CategoryUpdateDto
{
    public int Id { get; set; }
    public int? Order { get; set; }
    public int? ParentId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public int? ProcessId { get; set; }
    public string Description { get; set; }
    public string AttachmentDescription { get; set; }
    public int? Duration { get; set; }
    public int? ResponseDuration { get; set; }
    public CategoryType? CategoryType { get; set; }
    public bool ObjectionAllowed { get; set; }
    public bool IsDeleted { get; set; }
}

//this dto moved to Contracts.ReportDtos.cs
//public class CreateReportDto
//{
//    public int CategoryId { get; set; }
//    public string Comments { get; set; } = string.Empty;
//    public AddressDto Address { get; set; } = null!;
//    public List<Guid> Attachments { get; set; } = new List<Guid>();
//    public bool IsIdentityVisible { get; set; } = true;
//}

//this dto moved to Contracts.ReportDtos.cs (with some changes)
//public class UpdateReportDto
//{
//    public Guid Id { get; set; }
//    public int? CategoryId { get; set; }
//    public string? Comments { get; set; }
//    public bool? IsIdentityVisible { get; set; }
//    public Visibility? Visibility { get; set; }
//    public AddressDto? Address { get; set; }
//    public List<Guid>? Attachments { get; set; }
//}

//this dto moved to Contracts.ReportDtos.cs
//public class OperatorCreateReportDto
//{
//    public int CategoryId { get; set; }
//    public string Comments { get; set; } = null!;
//    public string PhoneNumber { get; set; } = null!;
//    public string FirstName { get; set; } = string.Empty;
//    public string LastName { get; set; } = string.Empty;
//    public bool IsIdentityVisible { get; set; }
//    public AddressDto Address { get; set; } = null!;
//    public List<Guid> Attachments { get; set; } = new List<Guid>();

//    //These are specific for verifying the report by operator
//    public Guid? Id { get; set; }

//    public ICollection<Media> Medias { get; set; } = new List<Media>();

//    public Visibility Visibility { get; set; } = Visibility.Operators;
//}



public class RegionGetDto
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; }
    public int CityId { get; set; }
}
public class CitizenGetReportDto
{
    public Guid Id { get; set; }
    public DateTime Sent { get; set; }
    public CategoryDto Category { get; set; }
    public AddressDto Address { get; set; }
    public string TrackingNumber { get; set; }
    public string Comments { get; set; }
    public string LastStatus { get; set; }
    public int Likes { get; set; }
    public bool IsLiked { get; set; }
    public ICollection<Media> Medias { get; set; }
    public IEnumerable<TransitionLogDto> TransitionLogs { get; set; }
    public ICollection<GetMessageDto> Messages { get; set; }
    public int CommentsCount { get; set; }
    public ReportState ReportState { get; set; }
    public ApplicationUserDto Registrant { get; set; }
    public ApplicationUserDto Citizen { get; set; }
    public bool IsIdentityVisible { get; set; }
    public int? Rating { get; set; }
}

public class AdminGetReportDto
{
    public Guid Id { get; set; }
    public DateTime Sent { get; set; }
    public DateTime? Finished { get; set; }
    public DateTime? Responsed { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? ResponseDeadline { get; set; }
    public DateTime LastStatusDateTime { get; set; }
    public string LastStatus { get; set; }
    public string CitizenId { get; set; }
    public ApplicationUserDto Citizen { get; set; }
    public ApplicationUserDto Registrant { get; set; }
    public int CategoryId { get; set; }
    public CategoryDto Category { get; set; }
    public Guid AddressId { get; set; }
    public AddressDto Address { get; set; }
    public string TrackingNumber { get; set; }
    public int? PriorityId { get; set; }
    public Priority Priority { get; set; }
    public int VisibilityId { get; set; }
    public Visibility Visibility { get; set; }
    public ICollection<Media> Medias { get; set; }
    public string Comments { get; set; }
    public int CommentsCount { get; set; }
    public ReportState ReportState { get; set; }

    //Process infos
    public int? ProcessId { get; set; }
    public ProcessDto Process { get; set; }
    public int CurrentStageId { get; set; }
    public StageDto CurrentStage { get; set; }

    //public ICollection<TransitionLog> TransitionLogs { get; set; }
    public int Likes { get; set; }

    public double? Duration { get; set; }
    public double? ResponseDuration { get; set; }
    public bool IsIdentityVisible { get; set; }
    public int? Rating { get; set; }
}

public class ReportDto
{
    public Guid Id { get; set; }
    public DateTime Sent { get; set; }
    public DateTime? Finished { get; set; }
    public DateTime? Responsed { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? ResponseDeadline { get; set; }
    public DateTime LastStatusDateTime { get; set; }
    public string LastStatus { get; set; }
    public string CitizenId { get; set; }
    public ApplicationUserDto Citizen { get; set; }
    public ApplicationUserDto Registrant { get; set; }
    public int CategoryId { get; set; }
    public CategoryDto Category { get; set; }
    public Guid AddressId { get; set; }
    public AddressDto Address { get; set; }
    public int ProcessId { get; set; }
    public string TrackingNumber { get; set; }
    public int? PriorityId { get; set; }
    public Priority Priority { get; set; }
    public int VisibilityId { get; set; }
    public Visibility Visibility { get; set; }
    public string Comments { get; set; }
    public ICollection<Media> Medias { get; set; }
    public ICollection<IFormFile> Attachments { get; set; }
    public bool IsIdentityVisible { get; set; }
    public int? Rating { get; set; }
    public int CommentsCount { get; set; }
    public int Likes { get; set; }
}

public class GetReportDto
{
    public Guid Id { get; set; }
    public DateTime Sent { get; set; }
    public DateTime LastStatusDateTime { get; set; }
    public string LastStatus { get; set; }
    public ApplicationUserDto Citizen { get; set; }
    public CategoryDto Category { get; set; }
    public AddressDto Address { get; set; }
    public string TrackingNumber { get; set; }
    public Visibility Visibility { get; set; }
    public Priority Priority { get; set; }
    public string Comments { get; set; }
    public ICollection<Media> Medias { get; set; }
    public DateTime Deadline { get; set; }
    public DateTime? ResponseDeadline { get; set; }
    public bool IsIdentityVisible { get; set; }
    public int? Rating { get; set; }
    public int CommentsCount { get; set; }
    public int Likes { get; set; }
}

public class ApplicationUserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public string Organization { get; set; }
    public Media Avatar { get; set; }
    public string PhoneNumber { get; set; }
    public string PhoneNumber2 { get; set; }
    public string NationalId { get; set; }
    public Gender Gender { get; set; }
    public int? EducationId { get; set; }
    //Navigation property
    public Education Education { get; set; }
    public DateTime BirthDate { get; set; }
    public string CitizenshipCode { get; set; }
    public AddressDto Address { get; set; }
}

public class ApplicationUserRestrictedDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public string Organization { get; set; }
    public Media Avatar { get; set; }
}

public class GetTaskDto
{
    public GetReportDto Report { get; set; }
    //this dto in below is from ReportDtos
    public List<GetPossibleTransitionDto> PossibleTransitions { get; set; }
}

//these dtos there are in ReportDtos.cs 

//public class PossibleTransitionDto
//{
//    public string StageTitle { get; set; }
//    public int TransitionId { get; set; }
//    public IEnumerable<ReasonDto> ReasonList { get; set; }
//    public IEnumerable<ActorDto> Actors { get; set; }
//    public bool CanSendMessageToCitizen { get; set; }
//    public TransitionType TransitionType { get; set; }
//}

//public class ReasonDto
//{
//    public int Id { get; set; }
//    public string Title { get; set; }
//    public string Description { get; set; }
//}
//public class ActorDto
//{
//    public int Id { get; set; }
//    public string Identifier { get; set; }
//    public ActorType Type { get; set; }
//    public string FirstName { get; set; }
//    public string LastName { get; set; }
//    public string Title { get; set; }
//    public string DisplayName { get { return Title + ((FirstName + LastName).Length > 0 ? " (" + FirstName + " " + LastName + ")" : ""); } }
//    public string Organization { get; set; }
//    public string PhoneNumber { get; set; }
//    public List<ActorDto> Actors { get; set; }
//}

public class NextStageDto
{
    public string Title { get; set; }
    public int TransitionId { get; set; }
    public ProcessStage Stage { get; set; }
}

public class GetUserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public string Organization { get; set; }
    public Media Avatar { get; set; }
    public string PhoneNumber { get; set; }
    public string PhoneNumber2 { get; set; }
    public string NationalId { get; set; }
    public Gender Gender { get; set; }

    //Navigation property
    public Education Education { get; set; }
    public DateTime? BirthDate { get; set; }
    public string CitizenshipCode { get; set; }
    public AddressDto Address { get; set; }
}

public class UpdateUserDto
{
    public string Id { get; set; }
    //public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public string Organization { get; set; }
    public IFormFile AvatarFile { get; set; }
    //public string PhoneNumber { get; set; }
    public string PhoneNumber2 { get; set; }
    public string NationalId { get; set; }
    public Gender? Gender { get; set; }
    public int? EducationId { get; set; }
    public DateTime BirthDate { get; set; }
    public string CitizenshipCode { get; set; }
    public AddressDto Address { get; set; }
}

public class MakeTransitionModel
{
    public int TransitionId { get; set; }
    public ICollection<ActorDto> Actors { get; set; }
    public string Comment { get; set; }
    public ICollection<Guid> Attachments { get; set; } = new List<Guid>();
    public ActorType ActorType { get; set; }
    public string ActorIdentifier { get; set; }
    public int? ReasonId { get; set; }
    public string MessageToCitizen { get; set; }
    public Visibility? Visibility { get; set; }
    //public Priority? Priority { get; set; }
}

/*
public class MakeTransitionDto
{
    public int TransitionId { get; set; }
    public ICollection<ActorDto> Actors { get; set; }
    public string Comment { get; set; }
    public ICollection<IFormFile> Attachments { get; set; }
    public ActorType? ActorType { get; set; }
    public string ActorIdentifier { get; set; }
    public int? ReasonId { get; set; }
    public string MessageToCitizen { get; set; }
    public Visibility? Visibility { get; set; }
    //public Priority Priority { get; set; }
}
*/

public class MoveToStageModel
{
    public bool IsAccepted { get; set; }
    public int StageId { get; set; }
    public ICollection<ActorDto> Actors { get; set; }
    public string Comment { get; set; }
    public ICollection<Media> Medias { get; set; }
    public ActorType ActorType { get; set; }
    public string ActorIdentifier { get; set; }
    public Visibility? Visibility { get; set; }
    //public Priority? Priority { get; set; }
}

//this dto moved to Contracts.ReportDtos.cs (with some changes)
//public class MoveToStageDto
//{
//    public bool IsAccepted { get; set; }
//    public int StageId { get; set; }
//    public ICollection<ActorDto> Actors { get; set; }
//    public string Comment { get; set; }
//    public ICollection<IFormFile> Attachments { get; set; }
//    public ActorType? ActorType { get; set; }
//    public string ActorIdentifier { get; set; }
//    public Visibility? Visibility { get; set; }
//    //public Priority Priority { get; set; }
//}

public class ObjectionDto
{
    public bool IsObjection { get; set; }
    public string Comment { get; set; }
    public ICollection<IFormFile> Attachments { get; set; }
}

public class ObjectionModel
{
    public bool IsObjection { get; set; }
    public string Comment { get; set; }
    public ICollection<Media> Medias { get; set; }
}

public class ReportActionDetailsDto
{
    public GetReportDto Report { get; set; }
    //this dto in below is from ReportDtos
    public IEnumerable<GetPossibleTransitionDto> PossibleTransitions { get; set; }
    public IEnumerable<StageDto> PossibleStages { get; set; }
    public IEnumerable<TransitionLogDto> History { get; set; }
}

public class AdminReportActionDetailsDto
{
    public AdminGetReportDto Report { get; set; }
    public IEnumerable<TransitionLogDto> History { get; set; }
}

//this dto moved to ReportDto.cs

//public class TransitionLogDto
//{
//    public Guid Id { get; set; }
//    public DateTime DateTime { get; set; }
//    public string Comment { get; set; }
//    public string Message { get; set; }
//    public ICollection<Media> Attachments { get; set; } = new List<Media>();
//    public ReasonDto Reason { get; set; }
//    public ActorType ActorType { get; set; }
//    public string ActorIdentifier { get; set; }
//    public bool IsPublic { get; set; }
//    public ActorDto Actor { get; set; }
//}

public class AdministrativeDivisionsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ParsimapCode { get; set; }
}

public class GetMessageDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DateTime { get; set; }
    public MessageType MessageType { get; set; }
    public Guid SubjectId { get; set; }
    public string FromId { get; set; }
}

public class GetStageDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class PriorityDto
{
    public int Id { get; set; }
    public int Value { get; set; }
    public string Name { get; set; }
}

public class ActorForActorsDto
{
    public int Id { get; set; }
    public string Identifier { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public string Organization { get; set; }
    public ActorType Type { get; set; }
    public IEnumerable<int> RegionIds { get; set; }
    public ICollection<RegionDto> Regions { get; set; } = new List<RegionDto>();
}

public class UserAndRegionDto
{
    public string Id { get; set; }
    public IEnumerable<int> RegionIds { get; set; }
}

public class ActorAndRegionDto
{
    public int Id { get; set; }
    public IEnumerable<int> RegionIds { get; set; }
}

public class RegionDto
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; }
    public int CityId { get; set; }
    public ICollection<ActorDto> Actors { get; set; }
}

public class OrganizationalUnitDto
{
    public int Id { get; set; }
    public OrganizationalUnitType? Type { get; set; }
    public string UserId { get; set; }
    public ApplicationUserDto User { get; set; }
    public int? ActorId { get; set; }
    public ActorDto Actor { get; set; }
    public string Title { get; set; }
    public int? ParentId { get; set; }
    public OrganizationalUnitDto Parent { get; set; }
    public List<OrganizationalUnitDto> OrganizationalUnits { get; set; } = new List<OrganizationalUnitDto>();
}

public class OrganizationalUnitCreateDto
{
    public int Id { get; set; }
    public OrganizationalUnitType? Type { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int? ActorId { get; set; }
    public ActorDto Actor { get; set; }
    public string Title { get; set; }
    public List<int> OrganizationalUnitIds { get; set; } = new List<int>();
    public List<int> ActorIds { get; set; } = new List<int>();
}

public class OrganizationalUnitUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<int> OrganizationalUnitIds { get; set; } = new List<int>();
    public List<int> ActorIds { get; set; } = new List<int>();
}

public class IdNameDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class UpdateReportCommentsDto
{
    public string Comments { get; set; }
}

public class GetMessageCountDto
{
    public long Count { get; set; }
    public DateTime DateTime { get; set; }
}
