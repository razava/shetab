using Api.Services;

namespace Api.Configurations;

public class AppSettings
{
    //TODO: Should be nullable?
    public GeneralSettings? GeneralSettings { get; set; } = null!;
    public AppVersions? AppVersions { get; set; } = null!;
    public FeedbackOptions? FeedbackOptions { get; set; } = null!;
    public FirebaseProxyOptions? FirebaseProxyOptions { get; set; } = null!;
    public ImageQualityOptions? ImageQualityOptions { get; set; } = null!;
    public ParsiMapOptions? ParsiMapOptions { get; set; } = null!;
    public SmsOptions? SmsOptions { get; set; } = null!;
    public GovOptions? GovOptions { get; set; } = null!;

}
