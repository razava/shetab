using Application.Categories.Common;
using Domain.Models.Relational.Common;

namespace Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(
    int Id,
    string? Code = null,
    string? Title = null,
    string? Description = null,
    int? Order = null,
    int? ParentId = null,
    int? Duration = null,
    int? ResponseDuration = null,
    int? ProcessId = null,
    bool? IsDeleted = null,
    bool? ObjectionAllowed = null,
    bool? EdittingAllowed = null,
    bool? HideMap = null,
    string? AttachmentDescription = null,
    Guid? FormId = null,
    Priority? DefaultPriority = null) : IRequest<Result<CategoryDetailResponse>>;

