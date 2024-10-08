﻿namespace Infrastructure.BackgroundJobs;

public class FeedbackOptions
{
    public const string Name = "FeedbackOptions";
    public string BaseUrl { get; set; } = null!;
    public int WaitTime { get; set; }
    public int RetryLimit { get; set; }
    public int CheckInterval { get; set; }
}
