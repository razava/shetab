using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.Dtos.ParsiMap
{
    public class Polyline
    {
        [JsonPropertyName("points")]
        public string Points { get; set; }
    }

    public class NameValue
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("value")]
        public int Value { get; set; }
    }

    public class Step
    {
        [JsonPropertyName("polyline")]
        public Polyline Polyline { get; set; }

        [JsonPropertyName("travel_mode")]
        public string TravelMode { get; set; }

        [JsonPropertyName("distance")]
        public NameValue Distance { get; set; }

        [JsonPropertyName("duration")]
        public NameValue Duration { get; set; }

        [JsonPropertyName("start_location")]
        public Location StartLocation { get; set; }

        [JsonPropertyName("end_location")]
        public Location EndLocation { get; set; }
    }

    public class Leg
    {
        [JsonPropertyName("steps")]
        public List<Step> Steps { get; set; }

        [JsonPropertyName("distance")]
        public NameValue Distance { get; set; }

        [JsonPropertyName("duration")]
        public NameValue Duration { get; set; }

        [JsonPropertyName("start_location")]
        public Location StartLocation { get; set; }

        [JsonPropertyName("end_location")]
        public Location EndLocation { get; set; }
    }

    public class Route
    {
        [JsonPropertyName("legs")]
        public List<Leg> Legs { get; set; }

        [JsonPropertyName("distance")]
        public NameValue Distance { get; set; }

        [JsonPropertyName("duration")]
        public NameValue Duration { get; set; }

        [JsonPropertyName("start_location")]
        public Location StartLocation { get; set; }

        [JsonPropertyName("end_location")]
        public Location EndLocation { get; set; }
    }

    public class RoutingResult
    {
        [JsonPropertyName("routes")]
        public List<Route> Routes { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }


}
