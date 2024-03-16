namespace Application.Maps.Queries.MapBackwardQuery;

public record MapBackwardQuery(int instanceId, double Longitude, double Latitude):IRequest<Result<AddressResult>>;
public record AddressResult(int InstanceId, string Address, int RegionId);
