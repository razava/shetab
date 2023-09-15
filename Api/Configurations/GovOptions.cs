namespace Api.Configurations;

public class GovOptions
{
    public const string Name = "GovOptions";
    public string ClientId { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public string TokenUrl { get; set; } = null!;
    public string RedirectUrl { get; set; } = null!;
    public string UserInfoUrl { get; set; } = null!;
    public string GovUrl { get; set; } = null!;
}
