using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.Dtos.ParsiMap
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Location
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }
    }

    public class GeoLocation
    {
        [JsonPropertyName("south_west")]
        public Location SouthWest { get; set; }

        [JsonPropertyName("north_east")]
        public Location NorthEast { get; set; }

        [JsonPropertyName("center")]
        public Location Center { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }
    }

    public class Result
    {
        [JsonPropertyName("geo_location")]
        public GeoLocation GeoLocation { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class ForwardResult
    {
        [JsonPropertyName("results")]
        public List<Result> Results { get; set; }

        [JsonPropertyName("search_type")]
        public string SearchType { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }


}
