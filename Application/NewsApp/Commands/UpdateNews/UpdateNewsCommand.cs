using Application.NewsApp.Common;
using Microsoft.AspNetCore.Http;

namespace Application.NewsApp.Commands.UpdateNews;

public sealed record UpdateNewsCommand(
    int Id,
    string? Title,
    string? Description,
    string? Url,
    IFormFile? Image,
    bool? IsDeleted) : IRequest<Result<bool>>;