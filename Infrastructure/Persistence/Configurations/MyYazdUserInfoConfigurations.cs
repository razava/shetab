using Domain.Models.MyYazd;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MyYazdUserInfoConfigurations : IEntityTypeConfiguration<MyYazdUserInfo>
{
    public void Configure(EntityTypeBuilder<MyYazdUserInfo> builder)
    {
        builder.OwnsOne(m => m.User);
    }
}
