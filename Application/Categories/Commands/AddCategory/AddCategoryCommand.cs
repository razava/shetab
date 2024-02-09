using Domain.Models.Relational;

namespace Application.Categories.Commands.AddCategory;

public sealed record AddCategoryCommand(
    int InstanceId,
    string Code,
    string Title,
    string Description,
    int Order,
    int ParentId,
    int Duration,
    int ResponseDuration,
    int? ProcessId = null,
    bool IsDeleted = false,
    bool ObjectionAllowed = true,
    bool EdittingAllowed = true,
    bool HideMap = false,
    string AttachmentDescription = "",
    Guid? FormId = null) : IRequest<Result<Category>>;

