using Application.Users.Common;

namespace Application.Users.Queries.GetUserRegions;

public record GetUserRegionsQuery(int InstanceId, string UserId) : IRequest<Result<List<IsInRegionModel>>>;
