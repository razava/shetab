using Application.Reports.Common;
using Domain.Models.Relational.Common;
using Mapster;
using NetTopologySuite.Geometries;
using Point = NetTopologySuite.Geometries.Point;

namespace Api.Configurations;

public class MapsterConfigurations
{
    public static void Config()
    {
        /*
        TypeAdapterConfig<AddressInfo, Address>.NewConfig()
            .Map(dest => dest.Location, src => new Point(new Coordinate(src.Longitude, src.Latitude)));
        */
            //.Map(dest => dest.Location, src => new Point(src.Longitude, src.Latitude));
        //var t = new Point(53.285548763070665, 31.132678978675045);
        
    }
}
