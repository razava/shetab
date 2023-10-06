using Domain.Models.Relational.Common;
using MediatR;

namespace Application.Medias.Commands.AddMedia;

public sealed record GetMediaQuery(
    Guid id) : IRequest<Media>;

