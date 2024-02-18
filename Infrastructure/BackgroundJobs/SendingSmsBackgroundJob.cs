using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class SendingSmsBackgroundJob(
    IMessageRepository messageRepository,
    ICommunicationService communicationService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var smses = await messageRepository.GetToSendSms(100);
        foreach (var sms in smses)
        {
            await communicationService.SendAsync(sms.PhoneNumber, sms.Content);
        }
        var messageIds = smses.Select(s => s.MessageId).ToList();
        if (messageIds.Any())
        {
            await messageRepository.SetAsSend(messageIds);
        }
    }
}
