using Application.Common.Interfaces.Persistence;
using Application.Reports.Common;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.Reports.Queries.GetCitizenReportById;

internal sealed class GetCitizenReportByIdQueryHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetCitizenReportByIdQuery, Result<GetReportByIdResponse>>
{
    public async Task<Result<GetReportByIdResponse>> Handle(
        GetCitizenReportByIdQuery request, CancellationToken cancellationToken)
    {
        var context = unitOfWork.DbContext.Set<Report>();
        var query = context.Where(r => r.Id == request.Id && r.CitizenId == request.UserId);
        var result = await query
            .AsNoTracking()
            .Select(GetReportByIdResponse.GetSelector())
            .SingleOrDefaultAsync();

        if (result is null)
            return NotFoundErrors.Report;

        return result;
    }
}