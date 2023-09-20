using Domain.Models.Relational;
using Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Medias.Commands.AddMedia;

public sealed record GetMediaQuery(
    Guid id) : IRequest<Media>;

