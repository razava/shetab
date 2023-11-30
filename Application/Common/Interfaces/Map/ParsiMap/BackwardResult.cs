using System.Text.Json.Serialization;

namespace Application.Common.Interfaces.Map.ParsiMap;

// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
public class DistrictMap
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class Subdivisions
{
    [JsonPropertyName("ostan")]
    public DistrictMap? Ostan { get; set; }

    [JsonPropertyName("shahrestan")]
    public DistrictMap? Shahrestan { get; set; }

    [JsonPropertyName("bakhsh")]
    public DistrictMap? Bakhsh { get; set; }

    [JsonPropertyName("shahr")]
    public DistrictMap? Shahr { get; set; }
}

public class Geofence
{
    [JsonPropertyName("id")]
    public object Id { get; set; } = null!;

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}

public class BackwardResultApplication
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("subdivision_prefix")]
    public string? SubdivisionPrefix { get; set; }

    [JsonPropertyName("local_address")]
    public string? LocalAddress { get; set; }

    [JsonPropertyName("approximate_address")]
    public string? ApproximateAddress { get; set; }

    [JsonPropertyName("subdivisions")]
    public Subdivisions? Subdivisions { get; set; }

    [JsonPropertyName("geofences")]
    public List<Geofence>? Geofences { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}
