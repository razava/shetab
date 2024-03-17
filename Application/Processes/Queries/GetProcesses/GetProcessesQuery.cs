using Application.Common.FilterModels;
using Application.Processes.Common;

namespace Application.Processes.Queries.GetProcesses;

public record GetProcessesQuery(
    int InstanceId,
    QueryFilterModel? FilterModel = default!) : IRequest<Result<List<GetProcessListResponse>>>;
