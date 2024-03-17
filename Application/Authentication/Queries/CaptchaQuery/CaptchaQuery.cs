using Application.Common.Interfaces.Security;

namespace Application.Medias.Commands.AddMedia;

public sealed record CaptchaQuery() : IRequest<Result<CaptchaResultModel>>;

