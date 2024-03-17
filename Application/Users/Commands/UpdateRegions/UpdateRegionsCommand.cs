using Application.Users.Common;

namespace Application.Users.Commands.UpdateRegions;

public record UpdateRegionsCommand(
    int InstanceId,
    string UserId,
    List<IsInRegionModel> Regions) : IRequest<Result<bool>>;
