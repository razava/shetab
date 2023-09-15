namespace Api.Services;

public class ParsiMapOptions
{
    public const string Name = "ParsiMapOptions";
    public string ApiToken { get; set; } = null!;
    public string MapToken { get; set; } = null!;
    public string District { get; set; } = null!;
    public string ForwardBaseAddress { get; set; } = null!;
    public string BackwardBaseAddress { get; set; } = null!;
    public string RoutingBaseAddress { get; set; } = null!;
}
