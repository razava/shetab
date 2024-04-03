using Application.Common.Interfaces.Persistence;
using Application.Processes.Common;

namespace Application.Processes.Queries.GetProcessById;

internal partial class GetProcessByIdQueryHandler(IProcessRepository processRepository) 
    : IRequestHandler<GetProcessByIdQuery, Result<GetProcessResponse>>
{

    public async Task<Result<GetProcessResponse>> Handle(GetProcessByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await processRepository.GetProcessById(request.Id, GetProcessResponse.GetSelector());

        if (result is null)
            return NotFoundErrors.Process;

        return result;
    }
}
