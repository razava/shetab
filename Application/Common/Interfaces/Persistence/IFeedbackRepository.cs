using Domain.Models.Relational.ReportAggregate;

namespace Application.Common.Interfaces.Persistence;

public interface IFeedbackRepository : IGenericRepository<Feedback>
{
    Task<List<FeedbackWithReciepient>> GetToSendFeedbacks(int count);
    Task SetAsSent(List<Guid> ids);
}

public record FeedbackWithReciepient(Guid ReportId, string PhoneNumber);