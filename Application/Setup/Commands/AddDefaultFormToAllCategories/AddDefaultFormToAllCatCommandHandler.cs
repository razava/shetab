using Application.Common.Interfaces.Persistence;
using Application.Forms.Common;
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

        if (defaultFormId == default(Guid))
        {
            //throw new Exception("Dafault form not found");
            List<FormElementModel> elements = new List<FormElementModel>()
            {
                new FormElementModel(
                    "text",
                    "توضیحات",
                    "توضیحات",
                    1,
                    "{\"id\":\"ffa754e5-cf02-41af-a044-7d9a9cb457cd\",\"elementType\":\"text\",\"elementCategory\":\"input\",\"props\":{\"label\":\"توضیحات\",\"placeholder\":\"\",\"type\":\"text\",\"editable\":true,\"disabled\":false,\"englishOnly\":false,\"style\":{}},\"items\":[]}")
            };

            var form = Form.Create(request.instanceId, "default", elements.Select(e => e.GetFormElement()).ToList());
            unitOfWork.DbContext.Set<Form>().Add(form);
            await unitOfWork.SaveAsync();
            if(form.Id == default(Guid))
            {
                throw new Exception("Dafault form failure");
            }
            defaultFormId = form.Id;
        }

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
