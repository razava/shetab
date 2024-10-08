﻿using Application.Forms.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;


public class LocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}


public class AddressDto
{
    [Required]
    public int RegionId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    [MaxLength(1024)]
    public string Detail { get; set; } = string.Empty;
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


public record CreateQuickAccessDto(
    [Required]
    int CategoryId,
    [Required] [MaxLength(1024)]
    string Title,
    [Required]
    int Order,
    IFormFile Image);

public record UpdateQuickAccessDto(
    int? CategoryId,
    [MaxLength(1024)]
    string? Title,
    int? Order,
    IFormFile? Image,
    bool? IsDeleted = false);

public record EducationDto(
    int Id,
    string Title);


public record GetExecutiveDto(
    int Id,
    string Title);



public record CreateNewsDto(
    [Required] [MaxLength(256)]
    string Title,
    [Required] [MaxLength(5*1024*1024)]
    string Description,
    [Required] [MaxLength(512)]
    string Url,
    IFormFile Image,
    bool IsDeleted);

public record UpdateNewsDto(
    [MaxLength(256)]
    string? Title,
    [MaxLength(5*1024*1024)]
    string? Description,
    [MaxLength(512)]
    string? Url,
    IFormFile? Image,
    bool? IsDeleted);


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


public record CreateProcessDto(
    [Required] [MaxLength(256)]
    string Title,
    [Required] [MaxLength(64)]
    string Code,
    List<int> ActorIds);


public record UpdateProcessDto(
    [MaxLength(256)]
    string? Title,
    [MaxLength(64)]
    string? Code,
    List<int>? ActorIds);


public record CreateFormDto(
    [Required] [MaxLength(256)]
    string Title,
    List<FormElementModel> Elements);

public record UpdateFormDto(
    [MaxLength(256)]
    string? Title,
    List<FormElementModel>? Elements);

