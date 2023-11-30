namespace Infrastructure.Map.ParsiMap;

public class ParsiMapOptions
{
    public const string Name = "ParsiMapOptions";
    public string? ApiToken { get; set; }
    public string? MapToken { get; set; }
    public string? District { get; set; }
    public string? ForwardBaseAddress { get; set; }
    public string? BackwardBaseAddress { get; set; }
    public string? RoutingBaseAddress { get; set; }
}