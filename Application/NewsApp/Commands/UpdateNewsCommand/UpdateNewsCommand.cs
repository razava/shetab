using Domain.Models.Relational;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.NewsApp.Commands.UpdateNewsCommand;

public sealed record UpdateNewsCommand(
    int Id,
    string? Title,
    string? Description,
    string? Url,
    IFormFile? Image,
    bool? IsDeleted) : IRequest<Result<News>>;