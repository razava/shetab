using Application.Common.Exceptions;
using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Commands.UpdateNewsCommand;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Infrastructure.Storage;
using MediatR;

namespace Application.Faqs.Commands.UpdateFaqCommand;

internal sealed class UpdateFaqCommandHandler : IRequestHandler<UpdateFaqCommand, Faq>
{
    private readonly IFaqRepository _faqRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFaqCommandHandler(
        IFaqRepository faqRepository,
        IUnitOfWork unitOfWork)
    {
        _faqRepository = faqRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Faq> Handle(UpdateFaqCommand request, CancellationToken cancellationToken)
    {
        var faq = await _faqRepository.GetSingleAsync(q => q.Id == request.Id);
        if (faq is null)
            throw new NotFoundException("سوال متداول");

        faq.Update(request.Question, request.Answer, request.IsDeleted);

        _faqRepository.Update(faq);
        await _unitOfWork.SaveAsync();

        return faq;
    }
}
