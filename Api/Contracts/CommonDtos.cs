using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;


public class LocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}


public record GetEnum(
    int Value,
    string Title);


public record QueryFilter(
    [MaxLength(64)]
    string? Query);


public record TimeFilter(
    DateTime? SentFromDate,
    DateTime? SentToDate);



//todo: Review this
public class MediaDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Url2 { get; set; } = string.Empty;
    public string Url3 { get; set; } = string.Empty;
    public string Url4 { get; set; } = string.Empty;
    public string AlternateText { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public MediaType MediaType { get; set; }
}



public class AddressDto
{
    [Required]
    public int RegionId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    [MaxLength(512)]
    public string Detail { get; set; } = string.Empty;//todo : nullable?

    //public string Street { get; set; } = string.Empty;
    //public string Valley { get; set; } = string.Empty;
    //public string Number { get; set; } = string.Empty;
    //public string PostalCode { get; set; } = string.Empty;
    //public double? Elevation { get; set; }
}

public record AddressDetailDto(
    string Detail);

public record AddressMoreDetailDto(
    string Detail,
    double? Latitude,
    double? Longitude);


public record AddressReportGet(
    string Detail,
    double? Latitude,
    double? Longitude,
    int? RegionId,
    RegionName Region/* ?? */);

public record RegionName(
    string Name);


public record GetUserRegionsDto(
    int RegionId,
    string RegionName);


public record GetRegionDto(
    int Id,
    string Name,
    string ParsimapCode);


public record GetRolesDto(
    string RoleName,
    string RoleTitle);



public record CitizenGetQuickAccess(
    int Id,
    string Title,
    int Order,
    int CategoryId,
    MediaDto Media);


public record AdminGetQuickAccess(
    int Id,
    string Title,
    int Order,
    int CategoryId,
    MediaDto Media,
    bool IsDeleted);


public record CreateQuickAccessDto(
    [Required]
    int CategoryId,
    [Required] [MaxLength(64)]
    string Title,
    [Required]
    int Order,
    IFormFile Image);

public record UpdateQuickAccessDto(
    int? CategoryId,
    [MaxLength(64)]
    string? Title,
    int? Order,
    IFormFile? Image,
    bool? IsDeleted = false);

public record EducationDto(
    int Id,
    string Title);

public record TaradodReason(
    int Id,
    string Name);


public record GetExecutiveDto(
    int Id,
    string Title);


public record GetMessageDto(
    Guid Id,
    string Title,
    string Content,
    DateTime DateTime,
    MessageType MessageType,
    Guid SubjectId,
    string FromId);

public record GetMessageCountDto(
    long Count,
    DateTime DateTime);


public record GetNewsDto(
    int Id,
    string Title,
    string Description,
    string Url,
    MediaDto ImageFile,
    DateTime Created,
    bool IsDeleted);

public record CreateNewsDto(
    [Required] [MaxLength(256)]
    string Title,
    [Required] [MaxLength(2048)]
    string Description,
    [Required] [MaxLength(512)]
    string Url,
    IFormFile Image,
    bool IsDeleted);

public record UpdateNewsDto(
    [MaxLength(256)]
    string? Title,
    [MaxLength(2048)]
    string? Description,
    [MaxLength(512)]
    string? Url,
    IFormFile? Image,
    bool? IsDeleted);


public record GetFaqsDto(
    int Id,
    string Question,
    string Answer,
    bool IsDeleted);

public record CreateFaqDto(
    [Required] [MaxLength(512)]
    string Question,
    [Required] [MaxLength(5120)]
    string Answer,
    bool IsDeleted);

public record UpdateFaqDto(
    [MaxLength(512)]
    string? Question,
    [MaxLength(5120)]
    string? Answer,
    bool? IsDeleted);


public record GetProcessListDto(
    int Id,
    string Title,
    string Code);

public record CreateProcessDto(
    [Required] [MaxLength(32)]
    string Title,
    [Required] [MaxLength(16)]
    string Code,
    List<int> ActorIds);

public record UpdateProcessDto(
    [MaxLength(32)]
    string? Title,
    [MaxLength(16)]
    string? Code,
    List<int>? ActorIds);

public record GetProcessDto(
    int Id,
    string Title,
    List<int> ActorIds);

public record GetExecutiveListDto(
    int Id,
    string Title,
    string DisplayName);








