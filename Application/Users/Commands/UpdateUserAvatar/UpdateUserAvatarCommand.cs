using Domain.Models.Relational.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.UpdateUserAvatar;

public record UpdateUserAvatarCommand(
    string UserId,
    IFormFile Avatar
    ) : IRequest<Media>;