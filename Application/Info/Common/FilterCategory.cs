namespace Application.Info.Common;

public record FilterCategory(
    int Id,
    int Order,
    string Code,
    string Title,
    ICollection<FilterCategory> Categories);