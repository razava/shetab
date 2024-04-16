using Application.Faqs.Queries.GetFaq;

namespace Application.Faqs.Commands.AddFaq;

public sealed record AddFaqCommand(
    int InstanceId,
    string Question,
    string Answer,
    bool IsDeleted) : IRequest<Result<GetFaqsResponse>>;