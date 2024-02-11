using Application.Forms.Common;
using Domain.Models.Relational;

namespace Application.Categories.Common;

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
    FormResponse? Form)
{
    public static CategoryDetailResponse FromCategory(Category category)
    {
        return new CategoryDetailResponse(
            category.Id,
            category.Order,
            category.ParentId,
            category.Code,
            category.Title,
            category.ProcessId,
            category.Description,
            category.AttachmentDescription,
            category.Duration,
            category.ResponseDuration,
            category.ObjectionAllowed,
            category.EditingAllowed,
            category.IsDeleted,
            category.HideMap,
            category.Form == null ? null : FormResponse.FromForm(category.Form));
    }
};
