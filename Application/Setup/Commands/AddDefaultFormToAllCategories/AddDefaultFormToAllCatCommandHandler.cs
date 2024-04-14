using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Setup.Commands.AddDefaultFormToAllCategories;

internal class AddDefaultFormToAllCatCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddDefaultFormToAllCatCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddDefaultFormToAllCatCommand request, CancellationToken cancellationToken)
    {
        var defaultFormId = await unitOfWork.DbContext.Set<Form>()
            .AsNoTracking().Where(f => f.Title == "default" && f.ShahrbinInstanceId == request.instanceId)
            .Select(f => f.Id).FirstOrDefaultAsync();

        if (defaultFormId == default(Guid)) throw new Exception("Dafault form not found");

        var categories = await unitOfWork.DbContext.Set<Category>()
            .Where(c => c.ShahrbinInstanceId == request.instanceId)
            .ToListAsync();

        foreach ( var category in categories )
        {
            category.FormId = defaultFormId;
        }

        unitOfWork.DbContext.Set<Category>().AttachRange(categories);
        await unitOfWork.SaveAsync();

        return true;
    }
}
