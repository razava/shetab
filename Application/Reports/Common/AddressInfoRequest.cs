using Domain.Models.Relational.Common;
using NetTopologySuite.Geometries;

namespace Application.Reports.Common;

public record AddressInfoRequest(
    int RegionId,
    double Latitude,
    double Longitude,
    string PostalCode = "",
    string Number = "",
    string Street = "",
    string Valley = "",
    string Detail = "",
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