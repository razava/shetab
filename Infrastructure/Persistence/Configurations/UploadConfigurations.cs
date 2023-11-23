using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UploadConfigurations : IEntityTypeConfiguration<Upload>
{
    
    public void Configure(EntityTypeBuilder<Upload> builder)
    {
        builder.OwnsOne(u => u.Media);
        builder.HasOne(u => u.User)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

    }
}
