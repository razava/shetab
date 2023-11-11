using Domain.Models.Relational.Common;
using Domain.Models.Relational.ReportAggregate;

namespace Api.Contracts;

public class CategoryGetDto
{
    public int ShahrbinInstanceId { get; set; }
    public int Id { get; set; }
    public int Order { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public ICollection<CategoryGetDto> Categories { get; set; } = new List<CategoryGetDto>();
    public int? ProcessId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string AttachmentDescription { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int? ResponseDuration { get; set; }
    public CategoryType CategoryType { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<FormElement> FormElements { get; set; } = new List<FormElement>();
    public bool HideMap { get; set; }
}


//todo: Review this
public class MediaDto : Media { }

public class AddressDto
{
    public int? RegionId { get; set; }

    public string Street { get; set; } = string.Empty;
    public string Valley { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? Elevation { get; set; }
}

public record GetUserRegionsDto(
    int Id,
    int Code,
    string Name,
    int CityId);


public record CategoryTitleDto(
    string Title);

public record CategoryDetailDto(
    string Title,
    int? ResponseDuration,
    int Duration);

public record AddresDetailDto(
    string Detail);


public record AddresMoreDetailDto(
    string Detail,
    double? Latitude,
    double? Longitude);

public record CitizenGetQuickAccess(
    int Id,
    string Title,
    int Order,
    int CategoryId,
    MediaDto Media);



