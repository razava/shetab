using Api.Contracts;
using Domain.Models.Relational.Common;
using Mapster;

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

    }
}
