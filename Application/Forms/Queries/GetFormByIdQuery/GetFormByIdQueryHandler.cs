using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;

namespace Application.Forms.Queries.GetFormByIdQuery;

internal class GetFormByIdQueryHandler(IFormRepository formRepository) 
    : IRequestHandler<GetFormByIdQuery, Result<Form>>
{

    public async Task<Result<Form>> Handle(GetFormByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await formRepository.GetSingleAsync(f => f.Id == request.Id, false);
        if (result == null)
            return NotFoundErrors.Form;
        return result;

    }
}
