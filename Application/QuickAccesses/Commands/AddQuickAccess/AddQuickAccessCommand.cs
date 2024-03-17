using Domain.Models.Relational;
using Microsoft.AspNetCore.Http;

namespace Application.QuickAccesses.Commands.AddQuickAccess;

public sealed record AddQuickAccessCommand(
    int InstanceId,
    int CategoryId,
    string Title,
    IFormFile Image,
    int Order,
    bool IsDeleted) : IRequest<Result<QuickAccess>>;