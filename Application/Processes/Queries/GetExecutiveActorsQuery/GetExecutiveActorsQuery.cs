using MediatR;

namespace Application.Processes.Queries.GetExecutiveActorsQuery;

public record GetExecutiveActorsQuery() : IRequest<List<GetExecutiveActorsResponse>>;

