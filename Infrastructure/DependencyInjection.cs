using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Map;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Domain.Models.Relational.IdentityAggregate;
using Infrastructure.Authentication;
using Infrastructure.Captcha;
using Infrastructure.Communications;
using Infrastructure.Map;
using Infrastructure.Map.ParsiMap;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Storage;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SixLabors.ImageSharp;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        services.AddPersistence(configuration);
        services.AddRepositories();
        services.AddSecurity(configuration);
        services.AddStorage(configuration, webHostEnvironment);
        services.AddCommunication(configuration);
        services.AddMap(configuration);
        
        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
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
        services.AddScoped<IShahrbinInstanceRepository, ShahrbinInstanceRepository>();


        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtInfo = new JwtInfo(
            configuration["JWT:Secret"] ?? throw new Exception(),
            configuration["JWT:ValidIssuer"] ?? throw new Exception(),
            configuration["JWT:ValidAudience"] ?? throw new Exception(),
            new TimeSpan(24, 0, 0));

        services.AddScoped<IAuthenticationService>(x => new AuthenticationService(
            x.GetRequiredService<UserManager<ApplicationUser>>(),
            x.GetRequiredService<IUnitOfWork>(),
            jwtInfo));
        services.AddScoped<ICaptchaProvider, SixLaborsCaptchaProvider>();

        return services;
    }

    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        var imageSizes = configuration.GetSection(ImageQualityOptions.Name).GetSection("ImageQualities").Get<List<Size>>();
        services.AddScoped<IStorageService>(x => new StorageService(webHostEnvironment.WebRootPath, imageSizes));

        return services;
    }

    public static IServiceCollection AddCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string host = configuration["MessageBroker:Host"] ?? throw new Exception("Message broker configurations not found");
        string username = configuration["MessageBroker:Username"]! ?? throw new Exception("Message broker configurations not found");
        string password = configuration["MessageBroker:Password"]! ?? throw new Exception("Message broker configurations not found");
        // Add MassTransit as a service
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                //configurator.Message<MessageBrokerMessage>(x =>
                //{
                //    x.SetEntityName("shahrbin-communication");
                //});
                configurator.Host(new Uri(host), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                configurator.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<ICommunicationService, CommunicationServiceUsingMessageBroker>();
        //services.AddSingleton<ISmsService>(x => new KaveNegarSms(
        //    new KaveNegarInfo(
        //        "10008000600033",
        //        "6367746F52314D6A52574C4E5766372F76653278365466334B6F777A35463764732F765667653332396F593D",
        //        "Namay")));

        return services;
    }

    public static IServiceCollection AddMap(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ParsiMapOptions>(configuration.GetSection(ParsiMapOptions.Name));
        services.AddScoped<IMapService, ParsiMapService>();
        return services;
    }
}
