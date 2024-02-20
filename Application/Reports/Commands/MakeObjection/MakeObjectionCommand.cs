using Domain.Models.Relational;

namespace Application.Reports.Commands.MakeObjection;

public sealed record MakeObjectionCommand(
    string UserId,
    List<string> UserRoles,
    Guid ReportId,
    List<Guid> Attachments,
    string Comment) : IRequest<Result<Report>>;

