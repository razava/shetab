using Application.Common.Interfaces.Persistence;
using Application.Forms.Common;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Forms.Queries.GetFormById;

internal class GetFormByIdQueryHandler(IFormRepository formRepository)
    : IRequestHandler<GetFormByIdQuery, Result<FormResponse>>
{

    public async Task<Result<FormResponse>> Handle(GetFormByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await formRepository.GetFormById(request.Id, f => FormResponse.FromForm(f));

        if (result == null)
            return NotFoundErrors.Form;

        return result;

    }
}
