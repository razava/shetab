using Domain.Models.Relational.ReportAggregate;

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
    public ICollection<FormElement> FormElements { get; set; } = new List<FormElement>();
    public bool HideMap { get; set; }
}


//todo : review this ..................................
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
    public ICollection<FormElement> FormElements { get; set; } = new List<FormElement>();
    public bool ObjectionAllowed { get; set; }
    public bool EditingAllowed { get; set; } = true;
    public bool IsDeleted { get; set; }
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
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int? ProcessId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string AttachmentDescription { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int? ResponseDuration { get; set; }
    public ICollection<FormElement> FormElements { get; set; } = new List<FormElement>();
    public bool ObjectionAllowed { get; set; }
    public bool EditingAllowed { get; set; } = true;
    public bool HideMap { get; set; }
}



public record GetShortCategoryDto(
    int Id,
    string Code,
    string Title,
    ICollection<FormElement> FormElements);


public record CategoryTitleDto(
    string Title);

public record CategoryDetailDto(
    string Title,
    int? ResponseDuration,
    int Duration);





