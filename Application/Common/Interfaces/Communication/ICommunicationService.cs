using Domain.Models.Relational;

namespace Application.Common.Interfaces.Communication;

public interface ICommunicationService
{
    public void AddNotification(Message message);
}
