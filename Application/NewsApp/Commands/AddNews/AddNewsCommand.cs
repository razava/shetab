using Application.NewsApp.Common;
using Microsoft.AspNetCore.Http;

namespace Application.NewsApp.Commands.AddNews;

public sealed record AddNewsCommand(
    int InstanceId,
    string Title,
    string Description,
    string Url,
    IFormFile Image,
    bool IsDeleted) : IRequest<Result<GetNewsResponse>>;