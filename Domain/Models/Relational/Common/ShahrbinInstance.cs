namespace Domain.Models.Relational.Common;

public class ShahrbinInstance
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public int CityId { get; set; }
    public City City { get; set; } = null!;
    public string EnglishName { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
