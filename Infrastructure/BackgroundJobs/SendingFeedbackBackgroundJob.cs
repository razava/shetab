using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Infrastructure.Communications.UrlShortener;
using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public class SendingFeedbackBackgroundJob(
    IOptions<FeedbackOptions> feedbackOptions,
    IFeedbackRepository feedbackRepository,
    ICommunicationService communicationService,
    IUrlShortenerService urlShortenerService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var messageDescription = "درخواست شما در سامانه شهربین رسیدگی شد. لطفاً میزان رضایتمندی خود را از طریق لینک زیر با ما در میان بگذارید.";
        
        var feedbacks = await feedbackRepository.GetToSendFeedbacks(100);

        if (!feedbacks.Any())
            return;

        var urlsResult = await urlShortenerService.UrlShortener(
            feedbacks.Select(f => new ShortenUrlRequest("FEEDBACK", f.FeedbackId.ToString())).ToList());
        if (urlsResult.IsFailed)
            return;

        foreach (var sms in feedbacks)
        {
            var url = urlsResult.Value.Where(u => u.Path == sms.FeedbackId.ToString()).First();
            var message = $"{messageDescription}\n{url}";
            await communicationService.SendAsync(sms.PhoneNumber, message);
        }
        var messageIds = feedbacks.Select(s => s.FeedbackId).ToList();
        if (messageIds.Any())
        {
            await feedbackRepository.SetAsSent(messageIds);
        }
    }
}
