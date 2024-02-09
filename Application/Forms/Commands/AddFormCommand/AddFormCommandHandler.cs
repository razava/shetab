using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;

namespace Application.Forms.Commands.AddFormCommand;

internal sealed class AddFormCommandHandler(
    IFormRepository formRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddFormCommand, Result<Form>>
{

    public async Task<Result<Form>> Handle(AddFormCommand request, CancellationToken cancellationToken)
    {
        var form = Form.Create(request.InstanceId, request.Title, request.Elements);

        formRepository.Insert(form);
        await unitOfWork.SaveAsync();

        return form;
    }
}
