using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;

public class CategoryCreateDto
{
    [Required]
    public int Order { get; set; }
    [Required]
    public int ParentId { get; set; }
    [Required] [MaxLength(8)]
    public string Code { get; set; } = string.Empty;
    [Required] [MaxLength(1024)]
    public string Title { get; set; } = string.Empty;
    public int? ProcessId { get; set; }
    [MaxLength(1024)]
    public string Description { get; set; } = string.Empty;
    [MaxLength(1024)]
    public string AttachmentDescription { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int ResponseDuration { get; set; }
    public List<string>? OperatorIds { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? FormId { get; set; }
    public bool ObjectionAllowed { get; set; }
    public bool EditingAllowed { get; set; } = true;
    public bool HideMap { get; set; }
    public Priority DefaultPriority { get; set; } = Priority.Normal;
}

public class CategoryUpdateDto
{
    public int? Order { get; set; }
    public int? ParentId { get; set; }
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(1024)]
    public string? Title { get; set; }
    public int? ProcessId { get; set; }
    [MaxLength(1024)]
    public string? Description { get; set; }
    [MaxLength(1024)]
    public string? AttachmentDescription { get; set; }
    public int? Duration { get; set; }
    public int? ResponseDuration { get; set; }
    public bool? IsDeleted { get; set; }
    public Guid? FormId { get; set; }
    public bool? ObjectionAllowed { get; set; }
    public bool? EditingAllowed { get; set; }
    public bool? HideMap { get; set; }
    public Priority? DefaultPriority { get; set; }
}






