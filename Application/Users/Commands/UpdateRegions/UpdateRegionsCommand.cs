using Application.Users.Common;
using MediatR;

namespace Application.Users.Commands.UpdateRegions;

public record UpdateRegionsCommand(int InstanceId, string UserId, List<IsInRegionModel> Regions) : IRequest<bool>;
