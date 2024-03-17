using Application.Processes.Common;

namespace Application.Processes.Queries.GetProcessById;

public record GetProcessByIdQuery(int Id) : IRequest<Result<GetProcessResponse>>;
