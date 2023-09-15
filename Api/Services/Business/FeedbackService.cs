using Api.Configurations;
using Api.Services.Sms;
using Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Services.Business;

public class FeedbackService
{
    private readonly ApplicationDbContext _context;
    private readonly IOptions<FeedbackOptions> _feedbackOptions;
    private readonly CommunicationServices _communication;

    public FeedbackService(
        ApplicationDbContext context,
        IOptions<FeedbackOptions> feedbackOptions,
        CommunicationServices communication)
    {
        _context = context;
        _feedbackOptions = feedbackOptions;
        _communication = communication;
    }

    public async Task Schedule(int interval)
    {
        while (true)
        {
            await SendFeedbackRequest();
            await Task.Delay(interval);
        }
    }
    public async Task<int> SendFeedbackRequest()
    {
        var now = DateTime.Now;
        var threshold = now.Subtract(new TimeSpan(0, _feedbackOptions.Value.WaitTime, 0));
        var feedbacks = await _context.Feedback
            .Where(p => p.Creation <= threshold && p.Rating == null && p.TryCount < _feedbackOptions.Value.RetryLimit)
            .Include(p => p.Report)
            .ThenInclude(p => p.Citizen)
            .ToListAsync();

        foreach (var feedback in feedbacks)
        {
            //await _communication.SendFeedbackRequestAsync(
            //    feedback.Report.Citizen.PhoneNumber,
            //    $"{_feedbackOptions.Value.BaseUrl}/{feedback.Id}/{feedback.Token}");
            var receptor = feedback.Report.Citizen.PhoneNumber;
            if (receptor == null) continue;
            await _communication.SendFeedbackRequestAsync(
                receptor,
                $"{_feedbackOptions.Value.BaseUrl}/{feedback.ReportId}");
            feedback.TryCount++;
            feedback.LastSent = now;
            feedback.PhoneNumber = receptor;
            _context.Entry(feedback).State = EntityState.Modified;
        }

        await _context.SaveChangesAsync();

        return feedbacks.Count();
    }
}
