using Amazon.Runtime.Internal;
using Application.Info.Common;
using MediatR;

namespace Application.Info.Queries.GetInfoQuery;

public record GetInfoQuery(int Code, int InstanceId, string UserId) : IRequest<InfoModel>;

