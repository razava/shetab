using Application.Common.Interfaces.Persistence;
using Application.Forms.Common;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Forms.Queries.GetForm;

internal class GetFormQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetFormQuery, Result<List<FormListItemResponse>>>
{
    public async Task<Result<List<FormListItemResponse>>> Handle(
        GetFormQuery request,
        CancellationToken cancellationToken)
    {
        var result = await unitOfWork.DbContext.Set<Form>()
            .Select(f => new FormListItemResponse(f.Id, f.Title))
            .ToListAsync();

        return result;
    }
}
