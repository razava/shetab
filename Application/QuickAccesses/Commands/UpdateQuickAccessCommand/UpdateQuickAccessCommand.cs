using Domain.Models.Relational;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.QuickAccesses.Commands.UpdateQuickAccessCommand;

public sealed record UpdateQuickAccessCommand(
    int Id,
    int? CategoryId,
    string? Title,
    IFormFile? Image,
    int? Order,
    bool? IsDeleted) : IRequest<QuickAccess>;