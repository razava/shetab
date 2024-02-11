using Application.Common.Interfaces.Persistence;
using Application.Forms.Common;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Forms.Queries.GetFormByIdQuery;

internal class GetFormByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetFormByIdQuery, Result<FormResponse>>
{

    public async Task<Result<FormResponse>> Handle(GetFormByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<Form>()
            .Where(f => f.Id == request.Id)
            .Select(f => FormResponse.FromForm(f))
            .SingleOrDefaultAsync();
        if (result == null)
            return NotFoundErrors.Form;
        return result;

    }
}
