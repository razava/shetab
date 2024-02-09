namespace Application.Forms.Common;

public record FormResponse(Guid Id, string Title, IEnumerable<FormElementResponse> Elements);
