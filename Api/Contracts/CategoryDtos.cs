using Domain.Models.Relational.ReportAggregate;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;


public class CategoryGetDto
{
    //public int ShahrbinInstanceId { get; set; }
    public int Id { get; set; }
    public int Order { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public ICollection<CategoryGetDto> Categories { get; set; } = new List<CategoryGetDto>();
    //public int? ProcessId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string AttachmentDescription { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int? ResponseDuration { get; set; }
    //public CategoryType CategoryType { get; set; }
    public bool IsDeleted { get; set; }
    public bool ObjectionAllowed { get; set; }
    public bool EditingAllowed { get; set; } = true;
    public bool HideMap { get; set; }
    public Form? Form { get; set; }
}


public class CategoryGetDetailDto
{
    public int Id { get; set; }
    public int? Order { get; set; }
    public int? ParentId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int? ProcessId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string AttachmentDescription { get; set; } = string.Empty;
    public int? Duration { get; set; }
    public int? ResponseDuration { get; set; }
    //public CategoryType? CategoryType { get; set; }
    public bool ObjectionAllowed { get; set; }
    public bool EditingAllowed { get; set; } = true;
    public bool IsDeleted { get; set; }
    public bool HideMap { get; set; }
    public Form? Form { get; set; }
}


//todo : review this .................................
public record FlattenCategoryDto(
    int Id,
    string Title,
    CategoryGetDetailDto Category);


public record FlattenShortCategoryDto(
    int Id,
    string Title);



public class CategoryCreateDto
{
    [Required]
    public int Order { get; set; }
    [Required]
    public int ParentId { get; set; }
    [Required] [MaxLength(8)]
    public string Code { get; set; } = string.Empty;
    [Required] [MaxLength(32)]
    public string Title { get; set; } = string.Empty;
    public int? ProcessId { get; set; }
    [MaxLength(1024)]
    public string Description { get; set; } = string.Empty;
    [MaxLength(1024)]
    public string AttachmentDescription { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int ResponseDuration { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? FormId { get; set; }
    public bool ObjectionAllowed { get; set; }
    public bool EditingAllowed { get; set; } = true;
    public bool HideMap { get; set; }
}

public class CategoryUpdateDto
{
    public int? Order { get; set; }
    public int? ParentId { get; set; }
    [MaxLength(8)]
    public string Code { get; set; } = string.Empty;
    [MaxLength(32)]
    public string Title { get; set; } = string.Empty;
    public int? ProcessId { get; set; }
    [MaxLength(1024)]
    public string Description { get; set; } = string.Empty;
    [MaxLength(1024)]
    public string AttachmentDescription { get; set; } = string.Empty;
    public int? Duration { get; set; }
    public int? ResponseDuration { get; set; }
    public bool? IsDeleted { get; set; }
    public Guid? FormId { get; set; }
    public bool? ObjectionAllowed { get; set; }
    public bool? EditingAllowed { get; set; } = true;
    public bool? HideMap { get; set; }
}

public record GetShortCategoryDto(
    int Id,
    string Code,
    string Title,
    Form? Form);


public record CategoryTitleDto(
    string Title);

public record CategoryDetailDto(
    string Title,
    int? ResponseDuration,
    int Duration);





