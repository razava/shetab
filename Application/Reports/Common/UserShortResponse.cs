using Domain.Models.Relational.Common;

namespace Application.Reports.Common;

public record UserShortResponse(
    string FirstName,
    string LastName,
    Media? Avatar);
