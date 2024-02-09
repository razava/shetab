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
            .Select(f => new FormResponse(
                f.Id,
                f.Title,
                f.Elements.Select(fe => new FormElementResponse(
                    fe.ElementType,
                    fe.Name,
                    fe.Title,
                    fe.Order,
                    fe.Meta))))
            .SingleOrDefaultAsync();
        if (result == null)
            return NotFoundErrors.Form;
        return result;

    }
}
