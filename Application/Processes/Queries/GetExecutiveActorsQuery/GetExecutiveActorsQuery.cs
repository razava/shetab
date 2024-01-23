using MediatR;

namespace Application.Processes.Queries.GetExecutiveActorsQuery;

public record GetExecutiveActorsQuery(int InstanceId) : IRequest<Result<List<GetExecutiveActorsResponse>>>;

