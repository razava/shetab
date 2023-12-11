using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Faqs.Commands.AddFaqCommand;

internal sealed class AddFaqCommandHandler : IRequestHandler<AddFaqCommand, Faq>
{
    private readonly IFaqRepository _faqRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddFaqCommandHandler(
        IFaqRepository faqRepository,
        IUnitOfWork unitOfWork)
    {
        _faqRepository = faqRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Faq> Handle(AddFaqCommand request, CancellationToken cancellationToken)
    {
        var faq = Faq.Create(request.InstanceId, request.Question, request.Answer, request.IsDeleted);

        _faqRepository.Insert(faq);
        await _unitOfWork.SaveAsync();

        return faq;
    }
}
