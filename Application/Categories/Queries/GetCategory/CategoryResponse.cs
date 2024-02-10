using Application.Forms.Common;

namespace Application.Categories.Queries.GetCategory;

public record CategoryResponse(
    int Id,
    int Order,
    string Code,
    string Title,
    ICollection<CategoryResponse> Categories,
    string Description,
    string AttachmentDescription,
    int Duration,
    int? ResponseDuration,
    bool IsDeleted,
    bool ObjectionAllowed,
    bool EditingAllowed,
    bool HideMap,
    FormResponse? Form);
