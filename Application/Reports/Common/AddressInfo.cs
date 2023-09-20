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
    double? Elevation = null);