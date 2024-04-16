using Application.Common.Helper;
using Application.Common.Interfaces.Persistence;
using Application.Faqs.Queries.GetFaq;
using Domain.Models.Relational;
using Mapster;
using SharedKernel.Successes;

namespace Application.Faqs.Commands.AddFaq;

internal sealed class AddFaqCommandHandler(
    IFaqRepository faqRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddFaqCommand, Result<GetFaqsResponse>>
{

    public async Task<Result<GetFaqsResponse>> Handle(AddFaqCommand request, CancellationToken cancellationToken)
    {
        var faq = Faq.Create(request.InstanceId, request.Question, request.Answer, request.IsDeleted);

        faqRepository.Insert(faq);
        await unitOfWork.SaveAsync();

        return ResultMethods.GetResult(faq.Adapt<GetFaqsResponse>(), CreationSuccess.Faq);
    }
}
