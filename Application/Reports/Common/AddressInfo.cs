using Domain.Models.Relational.Common;
using NetTopologySuite.Geometries;

namespace Application.Reports.Common;

public record AddressInfo(
    int RegionId,
    string Street,
    string Valley,
    string Detail,
    string Number,
    string PostalCode,
    double Latitude,
    double Longitude,
    double? Elevation = null)
{
    public Point Location { get { return new Point(Longitude, Latitude) { SRID = 4326 }; } }

    public Address GetAddress()
    {
        return new Address()
        {
            RegionId = RegionId,
            Street = Street,
            Valley = Valley,
            Detail = Detail,
            Number = Number,
            PostalCode = PostalCode,
            Location = Location,
        };
    }
}