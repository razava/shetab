using Domain.Models.Relational;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.NewsApp.Commands.AddNewsCommand;

public sealed record AddNewsCommand(
    int InstanceId,
    string Title,
    string Description,
    string Url,
    IFormFile Image,
    bool IsDeleted) : IRequest<News>;