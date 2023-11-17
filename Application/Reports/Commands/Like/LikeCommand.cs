using MediatR;

namespace Application.Reports.Commands.Like;

public record LikeCommand(string UserId, Guid ReportId, bool? IsLiked) :IRequest<bool>;

