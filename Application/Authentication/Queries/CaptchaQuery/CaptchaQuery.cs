using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Medias.Commands.AddMedia;

public sealed record CaptchaQuery() : IRequest<CaptchaResultModel>;

