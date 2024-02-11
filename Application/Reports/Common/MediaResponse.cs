using Domain.Models.Relational.Common;

namespace Application.Reports.Common;

public record MediaResponse(
    Guid Id,
    string Url,
    string Url2,
    string Url3,
    string Url4,
    string AlternateText,
    string Title,
    MediaType MediaType);
