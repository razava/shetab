using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.BackgroundJobs;

/*
public class FeedbackService
{
    private readonly ApplicationDbContext _context;
    private readonly int _waitTime;
    private readonly int _retryLimit;
    private readonly string _baseUrl;

    //private readonly IOptions<FeedbackOptions> _feedbackOptions;
    //private readonly CommunicationServices _communication;

    public FeedbackService(
        ApplicationDbContext context,
        int waitTime,
        int retryLimit,
        string baseUrl,
        CommunicationServices communication)
    {
        _context = context;
        _communication = communication;
        _waitTime = waitTime;
        _retryLimit = retryLimit;
        _baseUrl = baseUrl;
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
        var threshold = now.Subtract(new TimeSpan(0, _waitTime, 0));
        var feedbacks = await _context.Feedback
            .Where(p => p.Creation <= threshold && p.Rating == null && p.TryCount < _retryLimit)
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
*/