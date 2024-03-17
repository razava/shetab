using Application.Processes.Common;
using MediatR;

namespace Application.Processes.Queries.GetExecutiveActors;

public record GetExecutiveActorsQuery(int InstanceId) : IRequest<Result<List<GetExecutiveActorsResponse>>>;

