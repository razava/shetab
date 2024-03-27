using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class SendingFeedbackBackgroundJob(
    IFeedbackRepository feedbackRepository,
    ICommunicationService communicationService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var messageDescription = "درخواست شما در سامانه شهربین رسیدگی شد. لطفاً میزان رضایتمندی خود را از طریق لینک زیر با ما در میان بگذارید.";
        var baseUrl = "";
        var feedbacks = await feedbackRepository.GetToSendFeedbacks(100);
        foreach (var sms in feedbacks)
        {
            var message = $"{messageDescription}\n{baseUrl}\\{sms.FeedbackId}";
            await communicationService.SendAsync(sms.PhoneNumber, message);
        }
        var messageIds = feedbacks.Select(s => s.FeedbackId).ToList();
        if (messageIds.Any())
        {
            await feedbackRepository.SetAsSent(messageIds);
        }
    }
}
