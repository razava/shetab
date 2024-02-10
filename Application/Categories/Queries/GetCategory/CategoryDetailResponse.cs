using Application.Forms.Common;

namespace Application.Categories.Queries.GetCategory;

public record CategoryDetailResponse(
    int Id,
    int? Order,
    int? ParentId,
    string Code,
    string Title,
    int? ProcessId,
    string Description,
    string AttachmentDescription,
    int? Duration,
    int? ResponseDuration,
    bool ObjectionAllowed,
    bool EditingAllowed,
    bool IsDeleted,
    bool HideMap,
    FormResponse? Form);
