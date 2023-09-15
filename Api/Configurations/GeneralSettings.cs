namespace Api.Configurations;

public class GeneralSettings
{
    public const string Name = "GeneralSettings";

    public bool SendFeedbackRequests { get; set; }
    public bool SendFirebasePushNotifications { get; set; }
    public int SendMessagesInterval { get; set; }
    public bool UseProxy { get; set; }
    public string ProxyUrl { get; set; } = null!;
    public string CityName { get; set; } = null!;
    public string CityNameEn { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
