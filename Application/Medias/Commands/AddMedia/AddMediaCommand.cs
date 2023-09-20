using Domain.Models.Relational;
using Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Medias.Commands.AddMedia;

public sealed record AddMediaCommand(
    IFormFile File, AttachmentType AttachmentType) : IRequest<Media>;

