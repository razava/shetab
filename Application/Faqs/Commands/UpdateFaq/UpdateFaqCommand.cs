using Domain.Models.Relational;

namespace Application.Faqs.Commands.UpdateFaq;

public sealed record UpdateFaqCommand(
    int Id,
    string? Question,
    string? Answer,
    bool? IsDeleted) : IRequest<Result<Faq>>;