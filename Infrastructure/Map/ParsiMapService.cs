using Application.Common.Interfaces.Map;
using Application.Common.Interfaces.Map.ParsiMap;
using DocumentFormat.OpenXml.Bibliography;
using Domain.Models.Relational.Common;
using Infrastructure.Map.ParsiMap;
using Mapster;
using System.Text.Json;

namespace Infrastructure.Map;

public class ParsiMapService : IMapService
{
    private readonly HttpClient _client;
    private readonly ParsiMapOptions _mapOptions;
    public ParsiMapService(ParsiMapOptions parsiMapOptions)
    {
        _client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = false
        });
        _mapOptions = parsiMapOptions;
    }
    public async Task<ForwardResultApplication> Forward(string address)
    {
        var url = $"{_mapOptions.ForwardBaseAddress}" +
                $"key={_mapOptions.ApiToken}" +
                $"&search_text={address}" +
                $"&district={_mapOptions.District}" +
                $"&only_in_district=true" +
                $"&subdivision=false" +
                $"&plate=true" +
                $"&request_id=false" +
                $"&search_precision=full_address";
        try
        {
            var response = await _client.GetAsync(url);
            var result = await JsonSerializer.DeserializeAsync<ForwardResult>(await response.Content.ReadAsStreamAsync());
            var r2 = result.Adapt<ForwardResultApplication>();
            return r2;
        }
        catch
        {
            throw;
        }
    }

    public async Task<BackwardResultApplication> Backward(double longitude, double latitude)
    {
        var url = $"{_mapOptions.BackwardBaseAddress}" +
                $"key={_mapOptions.ApiToken}" +
                $"&location={longitude},{latitude}" +
                $"&local_address=true" +
                $"&approx_address=true" +
                $"&subdivision=true" +
                $"&geofence_types=9100" +
                $"&plate=false" +
                $"&request_id=false";
        try
        {
            var response = await _client.GetAsync(url);
            var result = await JsonSerializer.DeserializeAsync<BackwardResult>(await response.Content.ReadAsStreamAsync());
            var r2 = result.Adapt<BackwardResultApplication>();
            return r2;
        }
        catch
        {
            throw;
        }
    }
}
