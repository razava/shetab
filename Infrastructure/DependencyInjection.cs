using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Domain.Models.Relational.IdentityAggregate;
using Infrastructure.Authentication;
using Infrastructure.Captcha;
using Infrastructure.Communications;
using Infrastructure.Communications.Sms;
using Infrastructure.Communications.Sms.Panels;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration.GetConnectionString("DefaultConnection"));
        services.AddRepositories();
        services.AddSecurity();
        services.AddStorage();
        services.AddCommunication();
        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, string? connectionString)
    {
        if (connectionString is null)
            throw new Exception("Connection string cannot be null.");
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                o =>
                {
                    o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    o.MigrationsAssembly("Infrastructure");
                }));
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository> ();
        services.AddScoped<IFeedbackRepository, FeedbackRepository> ();
        services.AddScoped<IMediaRepository, MediaRepository> ();
        services.AddScoped<IProcessRepository, ProcessRepository> ();
        services.AddScoped<IReportRepository, ReportRepository> ();
        services.AddScoped<IUnitOfWork, UnitOfWork> ();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<ICountyRepository, CountyRepository>();
        services.AddScoped<IDistrictRepository, DistrictRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IRegionRepository, RegionRepository>();


        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService>(x => new AuthenticationService(
            x.GetRequiredService<UserManager<ApplicationUser>>(),
            x.GetRequiredService<IUnitOfWork>(),
            new JwtInfo("", "", "", new TimeSpan())));
        services.AddScoped<ICaptchaProvider, SixLaborsCaptchaProvider>();

        return services;
    }

    public static IServiceCollection AddStorage(this IServiceCollection services)
    {
        services.AddScoped<IStorageService>(x => new StorageService("", new List<Size>()));

        return services;
    }

    public static IServiceCollection AddCommunication(this IServiceCollection services)
    {
        services.AddSingleton<ICommunicationService, CommunicationService>();
        services.AddSingleton<ISmsService>(x => new KaveNegarSms(new KaveNegarInfo("", "", "")));

        return services;
    }
}
