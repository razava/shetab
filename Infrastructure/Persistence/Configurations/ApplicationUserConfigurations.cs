﻿using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.IdentityAggregate;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasOne(c => c.ShahrbinInstance)
            .WithMany()
            .OnDelete(DeleteBehavior.SetNull);

        builder.OwnsOne(u => u.Avatar);
        builder.OwnsOne(u => u.Address);
        builder.HasMany(u => u.Reports)
            .WithOne(r => r.Citizen)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(u => u.RegisteredReports)
            .WithOne(r => r.Registrant)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(u => u.ExecutiveReports)
            .WithOne(r => r.Executive)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(u => u.ContractorReports)
            .WithOne(r => r.Contractor)
            .OnDelete(DeleteBehavior.NoAction);
        builder.HasMany(u => u.InspectorReports)
            .WithOne(r => r.Inspector)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.HasMany(u => u.Executives)
            .WithMany(u => u.Contractors)
            .UsingEntity<ExecutiveContractor>(
                ec => ec.HasOne(e => e.Executive).WithMany().HasForeignKey(e => e.ExecutiveId).HasPrincipalKey(u => u.Id),
                ec => ec.HasOne(c => c.Contractor).WithMany().HasForeignKey(c => c.ContractorId).HasPrincipalKey(u => u.Id),
                j => j.HasKey(e=> new { e.ExecutiveId, e.ContractorId}));
        builder.HasMany(u => u.ReportsLiked)
            .WithMany(r => r.LikedBy)
            .UsingEntity<ReportLikes>(
                rl => rl.HasOne(u => u.Report).WithMany().HasForeignKey(u => u.ReportId).HasPrincipalKey(r => r.Id),
                rl => rl.HasOne(r => r.LikedBy).WithMany().HasForeignKey(r => r.LikedById).HasPrincipalKey(u => u.Id),
                j => j.HasKey(e => new { e.LikedById, e.ReportId }));

        builder.HasMany(u => u.Categories)
            .WithMany(c => c.Users)
            .UsingEntity<OperatorCategory>(
                rl => rl.HasOne(u => u.Category).WithMany().HasForeignKey(u => u.CategoryId).HasPrincipalKey(c => c.Id),
                rl => rl.HasOne(c => c.Operator).WithMany().HasForeignKey(c => c.OperatorId).HasPrincipalKey(u => u.Id),
                j => j.HasKey(e => new { e.CategoryId, e.OperatorId }));
    }
}
