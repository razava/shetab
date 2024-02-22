using Application.Forms.Common;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.Linq.Expressions;

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
    FormResponse? Form,
    Priority DefaultPriority)
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
            FormResponse.FromForm(category.Form),
            category.DefaultPriority);
    }

    public static Expression<Func<Category, CategoryDetailResponse>> GetSelector()
    {
        Expression<Func<Category, CategoryDetailResponse>> selector
            = category => new CategoryDetailResponse(
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
            FormResponse.FromForm(category.Form),
            category.DefaultPriority);
        return selector;
    }
};
