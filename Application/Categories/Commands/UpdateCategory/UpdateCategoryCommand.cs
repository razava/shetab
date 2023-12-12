using Domain.Models.Relational;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Categories.Commands.UpdateCategory;

public sealed record UpdateCategoryCommand(
    int id,
    string? Code,
    string? Title,
    string? Description,
    int? Order,
    int? ParentId,
    int? Duration,
    int? ResponseDuration,
    int? ProcessId = null,
    bool? IsDeleted = false,
    bool? ObjectionAllowed = true,
    bool? EdittingAllowed = true,
    bool? HideMap = false,
    string? AttachmentDescription = "",
    List<FormElement>? FormElements = null) : IRequest<Category>;

