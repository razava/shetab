using Application.Common.Interfaces.Map.ParsiMap;

namespace Application.Common.Interfaces.Map;

public interface IMapService
{
    public Task<BackwardResultApplication> Backward(double longitude, double latitude);
    public Task<ForwardResultApplication> Forward(string address);
}
