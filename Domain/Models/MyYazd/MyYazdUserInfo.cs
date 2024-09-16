using System.Text.Json.Serialization;

namespace Domain.Models.MyYazd;

public class MyYazdUserInfo
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; } = DateTime.UtcNow;
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int? ExpiresIn { get; set; }

    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("scope")]
    public string? Scope { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("user")]
    public MyYazdUser? User { get; set; }
}

public class MyYazdUser
{
    [JsonPropertyName("national_id")]
    public string? NationalId { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("mobile_no")]
    public string? MobileNo { get; set; }

    [JsonPropertyName("point")]
    public int? Point { get; set; }

    [JsonPropertyName("balance")]
    public int? Balance { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; }

    [JsonPropertyName("birthdate")]
    public string? Birthday { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("gender")]
    public int? Gender { get; set; }

    [JsonPropertyName("avatar")]
    public string? Avatar { get; set; }

    [JsonPropertyName("tel")]
    public string? Tel { get; set; }

    //[JsonPropertyName("level")]
    //public string? Level { get; set; }
}

