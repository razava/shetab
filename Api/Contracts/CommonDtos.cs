﻿using Domain.Models.Relational.Common;
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


//todo : review this ..................................
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


//todo : review this .................................
public record FlattenCategoryDto(
    int Id,
    string Title,
    CategoryUpdateDto Category);


//todo : review this .................................
public class CategoryCreateDto
{
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public int? ProcessId { get; set; }
    public string Description { get; set; }
    public string AttachmentDescription { get; set; }   //.....
    public int Duration { get; set; }
    public int? ResponseDuration { get; set; }
    //public ICollection<FormElement> FormElements { get; set; } = new List<FormElement>();
    public bool ObjectionAllowed { get; set; }
    //public bool EditingAllowed { get; set; } = true;
    public bool HideMap { get; set; }   //......
}



public record GetShortCategoryDto(
    int Id,
    string Code,
    string Title,
    ICollection<FormElement> FormElements);




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


public record AdminGetQuickAccess(
    int Id,
    string Title,
    int Order,
    int CategoryId,
    MediaDto Media,
    bool IsDeleted);


public record RegionName(
    string Name);

public record AddressReportGet(
    string Detail,
    double? Latitude,
    double? Longitude,
    RegionName Region);


public record GetRegionByName(
    int Id,
    string Name
    //,string ParsimapCode //??
    );


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


public record TimeFilter(
    DateTime? SentFromDate,
    DateTime? SentToDate);



public record GetNewsDto(
    int Id,
    string Title,
    string Description,
    string Url,
    MediaDto ImageFile,
    DateTime Created,
    bool IsDeleted);


public record QueryFilter(
    string Query);


