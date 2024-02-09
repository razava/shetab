namespace Application.Forms.Common;

public record FormElementResponse(
    string ElementType,
    string Name,
    string Title,
    int Order,
    string Meta);
