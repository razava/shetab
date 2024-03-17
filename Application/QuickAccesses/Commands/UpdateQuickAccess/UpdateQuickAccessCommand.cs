using Domain.Models.Relational;
using Microsoft.AspNetCore.Http;

namespace Application.QuickAccesses.Commands.UpdateQuickAccess;

public sealed record UpdateQuickAccessCommand(
    int Id,
    int? CategoryId,
    string? Title,
    IFormFile? Image,
    int? Order,
    bool? IsDeleted) : IRequest<Result<QuickAccess>>;