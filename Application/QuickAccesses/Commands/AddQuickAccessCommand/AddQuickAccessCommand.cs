using Domain.Models.Relational;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.QuickAccesses.Commands.AddQuickAccessCommand;

public sealed record AddQuickAccessCommand(
    int InstanceId,
    int CategoryId,
    string Title,
    IFormFile Image,
    int Order,
    bool IsDeleted) : IRequest<Result<QuickAccess>>;