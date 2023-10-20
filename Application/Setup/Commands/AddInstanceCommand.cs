using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Setup.Commands;

public sealed record AddInstanceCommand(List<IFormFile> files):IRequest<bool>;
