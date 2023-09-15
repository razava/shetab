namespace Domain.Models.Relational;

public class Address
{
    public Guid Id { get; set; }
    public int? RegionId { get; set; }
    public Region Region { get; set; } = null!;
    public string Street { get; set; } = string.Empty;
    public string Valley { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? Elevation { get; set; }
}

public class Province
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<County> Counties { get; set; } = new List<County>();
}

public class County
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;
    public List<District> Districts { get; set; } = new List<District>();
}

public class District
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; } = null!;
    public int CountyId { get; set; }
    public County County { get; set; } = null!;
    public List<City> Cities { get; set; } = new List<City>();
}

public class City
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; } = null!;
    public int DistrictId { get; set; }
    public District District { get; set; } = null!;
    public List<Region> Regions { get; set; } = new List<Region>();
}

public class Region
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; } = null!;
    public int CityId { get; set; }
    public City City { get; set; } = null!;
    public string ParsimapCode { get; set; } = string.Empty;
    public ICollection<Actor> Actors { get; set; } = null!;
}