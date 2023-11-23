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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        services.AddPersistence(configuration.GetConnectionString("DefaultConnection"));
        services.AddRepositories();
        services.AddSecurity(new JwtInfo(
            configuration["JWT:Secret"] ?? throw new Exception(),
            configuration["JWT:ValidIssuer"] ?? throw new Exception(),
            configuration["JWT:ValidAudience"] ?? throw new Exception(),
            new TimeSpan(24, 0, 0)));
        services.AddStorage(webHostEnvironment);
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
                    o.UseNetTopologySuite();
                }));
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork> ();
        services.AddScoped<ICategoryRepository, CategoryRepository> ();
        services.AddScoped<IFeedbackRepository, FeedbackRepository> ();
        services.AddScoped<IUploadRepository, UploadRepository> ();
        services.AddScoped<IProcessRepository, ProcessRepository> ();
        services.AddScoped<IReportRepository, ReportRepository> ();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProvinceRepository, ProvinceRepository>();
        services.AddScoped<ICountyRepository, CountyRepository>();
        services.AddScoped<IDistrictRepository, DistrictRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IRegionRepository, RegionRepository>();
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IViolationRepository, ViolationRepository>();
        services.AddScoped<IViolationTypeRepository, ViolationTypeRepository>();
        services.AddScoped<IQuickAccessRepository, QuickAccessRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IReportLikesRepository, ReportLikesRepository>();


        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services, JwtInfo jwtInfo)
    {
        services.AddScoped<IAuthenticationService>(x => new AuthenticationService(
            x.GetRequiredService<UserManager<ApplicationUser>>(),
            x.GetRequiredService<IUnitOfWork>(),
            jwtInfo));
        services.AddScoped<ICaptchaProvider, SixLaborsCaptchaProvider>();

        return services;
    }

    public static IServiceCollection AddStorage(this IServiceCollection services, IWebHostEnvironment webHostEnvironment)
    {
        //var temp = services.getrequiredservice<IWebHostEnvironment>();
        services.AddScoped<IStorageService>(x => new StorageService(/*"",*/webHostEnvironment, new List<Size>()));

        return services;
    }

    public static IServiceCollection AddCommunication(this IServiceCollection services)
    {
        services.AddSingleton<ICommunicationService, CommunicationService>();
        services.AddSingleton<ISmsService>(x => new KaveNegarSms(
            new KaveNegarInfo(
                "10008000600033",
                "6367746F52314D6A52574C4E5766372F76653278365466334B6F777A35463764732F765667653332396F593D",
                "Namay")));

        return services;
    }
}
