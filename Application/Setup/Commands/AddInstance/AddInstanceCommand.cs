using Microsoft.AspNetCore.Http;

namespace Application.Setup.Commands.AddInstance;

public sealed record AddInstanceCommand(List<IFormFile> files) : IRequest<bool>;
