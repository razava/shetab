using Domain.Models.Relational;

namespace Application.Faqs.Commands.AddFaq;

public sealed record AddFaqCommand(
    int InstanceId,
    string Question,
    string Answer,
    bool IsDeleted) : IRequest<Result<Faq>>;