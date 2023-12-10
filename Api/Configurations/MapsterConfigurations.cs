using Api.Contracts;
using Application.Processes.Queries.GetExecutiveActorsQuery;
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
        TypeAdapterConfig<Address, AddressMoreDetailDto>.NewConfig()
            .Map(dest => dest.Latitude, src => src.Location!.Y)
            .Map(dest => dest.Longitude, src => src.Location!.X);

        TypeAdapterConfig<Address, AddressReportGet>.NewConfig()
            .Map(dest => dest.Latitude, src => src.Location!.Y)
            .Map(dest => dest.Longitude, src => src.Location!.X);

        TypeAdapterConfig<GetExecutiveActorsResponse, GetExecutiveListDto>.NewConfig()
            .Map(dest => dest.DisplayName, src => $"{src.FirstName} {src.LastName}");

        /*
        TypeAdapterConfig<AddressInfo, Address>.NewConfig()
            .Map(dest => dest.Location, src => new Point(new Coordinate(src.Longitude, src.Latitude)));
        */
        //.Map(dest => dest.Location, src => new Point(src.Longitude, src.Latitude));
        //var t = new Point(53.285548763070665, 31.132678978675045);

    }
}
