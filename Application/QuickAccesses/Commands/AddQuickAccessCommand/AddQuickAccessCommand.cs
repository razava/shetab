using Domain.Models.Relational;
using MediatR;

namespace Application.QuickAccesses.Commands.AddQuickAccessCommand;

public sealed record AddQuickAccessCommand(
    int InstanceId,
    int CategoryId,
    string Title,
    Guid ImageId,
    int Order,
    bool IsDeleted) : IRequest<QuickAccess>;