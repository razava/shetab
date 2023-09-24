using Domain.Models.Relational;
using MediatR;

namespace Application.Medias.Commands.AddMedia;

public sealed record GetMediaQuery(
    Guid id) : IRequest<Media>;

