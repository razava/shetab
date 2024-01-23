using Application.Users.Common;
using MediatR;

namespace Application.Users.Queries.GetRegions;

public record GetUserRegionsQuery(int InstanceId, string UserId) : IRequest<Result<List<IsInRegionModel>>>;
