using Application.Forms.Common;
using Domain.Models.Relational;

namespace Application.Categories.Common;

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
    FormResponse? Form)
{
    public static CategoryResponse FromCategory(Category category)
    {
        return new CategoryResponse(
            category.Id,
            category.Order,
            category.Code,
            category.Title,
            new List<CategoryResponse>(),
            category.Description,
            category.AttachmentDescription,
            category.Duration,
            category.ResponseDuration,
            category.IsDeleted,
            category.ObjectionAllowed,
            category.EditingAllowed,
            category.HideMap,
            FormResponse.FromForm(category.Form));
    }
};