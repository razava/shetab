using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Microsoft.EntityFrameworkCore;

namespace Application.Forms.Commands.DeleteForm;

internal sealed class DeleteFormCommandHandler(
    IFormRepository formRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteFormCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeleteFormCommand request, CancellationToken cancellationToken)
    {
        var form = await formRepository.GetSingleAsync(q => q.Id == request.Id);
        if (form is null)
            return NotFoundErrors.Form;

        var isUsed = await unitOfWork.DbContext.Set<Category>().AnyAsync(c => c.FormId == request.Id);
        if (isUsed)
            return OperationErrors.FormIsInUse;

        formRepository.Delete(form);
        await unitOfWork.SaveAsync();

        return true;
    }
}
