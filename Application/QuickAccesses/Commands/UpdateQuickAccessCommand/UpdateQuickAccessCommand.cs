using Domain.Models.Relational;
using MediatR;

namespace Application.QuickAccesses.Commands.UpdateQuickAccessCommand;

public sealed record UpdateQuickAccessCommand(
    int Id,
    int? CategoryId,
    string? Title,
    Guid? ImageId,
    int? Order,
    bool? IsDeleted) : IRequest<QuickAccess>;