namespace Application.Reports.Common;

public record AddressResponse(
    string Detail,
    double? Latitude,
    double? Longitude,
    int? RegionId,
    string? RegionName);
