﻿using Application.Common.Interfaces.Caching;
using Application.Common.Interfaces.Communication;
using Application.Common.Interfaces.Info;
using Application.Common.Interfaces.Map;
using Application.Common.Interfaces.MyYazd;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Infrastructure.Authentication;
using Infrastructure.BackgroundJobs;
using Infrastructure.Caching;
using Infrastructure.Captcha;
using Infrastructure.Communications;
using Infrastructure.Communications.PushNotification;
using Infrastructure.Communications.UrlShortener;
using Infrastructure.Exceptions;
using Infrastructure.Info;
using Infrastructure.Map;
using Infrastructure.Map.ParsiMap;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Storage;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using StackExchange.Redis;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddRedis(configuration);
        services.AddRepositories();
        services.AddSecurity(configuration);
        services.AddStorage(configuration);
        services.AddCommunication(configuration);
        services.AddMap(configuration);
        services.AddCache();
        services.AddBackgroundJobs(configuration);
        services.AddMyYazd(configuration);
        services.AddScoped<IInfoService, InfoService>();
        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (connectionString is null)
            throw new NullConnectionStringException();
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

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
                 ConnectionMultiplexer.Connect(new ConfigurationOptions
                 {
                     EndPoints = { $"{configuration.GetValue<string>("Redis:Host")}:{configuration.GetValue<int>("Redis:Port")}" },
                     AbortOnConnectFail = false,
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
        services.AddScoped<IReportNoteRepository, ReportNoteRepository> ();
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
        services.AddScoped<INewsRepository, NewsRepository>();
        services.AddScoped<IFaqRepository, FaqRepository>();
        services.AddScoped<IFormRepository, FormRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IPollRepository, PollRepository>();
        services.AddScoped<IOrganizationalUnitRepository, OrganizationalUnitRepository>();
        services.AddScoped<ISatisfactionRepository, SatisfactionRepository>();


        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtInfo = configuration.GetSection("JWT").Get<JwtInfo>();
        if (jwtInfo is null)
            throw new Exception("Jwt info not exist");

        services.AddSingleton(jwtInfo);
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICaptchaProvider, SixLaborsCaptchaProvider>();
        services.AddScoped<IAuthenticateRepository, AuthenticateRepository>();

        return services;
    }

    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StorageOptions>(
            configuration.GetSection(StorageOptions.Name));
        services.AddScoped<IStorageService, StorageService>();

        return services;
    }

    public static IServiceCollection AddCommunication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string host = configuration["MessageBroker:Host"] ?? throw new MessageBrokerConfigurationsNotFoundException();
        string username = configuration["MessageBroker:Username"]! ?? throw new MessageBrokerConfigurationsNotFoundException();
        string password = configuration["MessageBroker:Password"]! ?? throw new MessageBrokerConfigurationsNotFoundException();
        // Add MassTransit as a service
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(host), h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                configurator.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<ICommunicationService, CommunicationServiceUsingMessageBroker>();

        services.AddSingleton<IFirebaseCloudMessaging>(new FirebaseCloudMessaging());


        services.Configure<UrlShortenerOptions>(
            configuration.GetSection(UrlShortenerOptions.Name));

        services.AddHttpClient<IUrlShortenerService, UrlShortenerService>(
            client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection(UrlShortenerOptions.Name).GetValue<string>("Url") ??
                    throw new Exception("Url shortener url not specified."));
            });

        return services;
    }

    public static IServiceCollection AddMap(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ParsiMapOptions>(configuration.GetSection(ParsiMapOptions.Name));
        services.AddScoped<IMapService, ParsiMapService>();
        return services;
    }

    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IQueryCacheService, QueryCacheService>();
        return services;
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(options =>
        {
            var smsCronSchedule = configuration.GetValue<string>("JobScheduleOptions:Sms") ?? "0 0/5 * ? * * *";
            var smsJobKey = JobKey.Create(nameof(SendingSmsBackgroundJob));
            options.AddJob<SendingSmsBackgroundJob>(smsJobKey, j => j.StoreDurably())
                .AddTrigger(trigger => 
                    trigger
                        .ForJob(smsJobKey)
                        .WithCronSchedule(smsCronSchedule));

            services.Configure<FeedbackOptions>(configuration.GetSection(FeedbackOptions.Name));
            var feedbackCronSchedule = configuration.GetValue<string>("JobScheduleOptions:Feedback") ?? "0 0 19 ? * * *";
            var feedbackJobKey = JobKey.Create(nameof(SendingFeedbackBackgroundJob));
            options.AddJob<SendingFeedbackBackgroundJob>(feedbackJobKey, j => j.StoreDurably())
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(feedbackJobKey)
                        .WithCronSchedule(feedbackCronSchedule));

            var statisticsCronSchedule = configuration.GetValue<string>("JobScheduleOptions:Statistics") ?? "0 0 7 ? * * *";
            var statisticsJobKey = JobKey.Create(nameof(SendingStatisticsBackgroundJob));
            options.AddJob<SendingStatisticsBackgroundJob>(statisticsJobKey, j => j.StoreDurably())
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(statisticsJobKey)
                        .WithCronSchedule(statisticsCronSchedule));
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }

    public static IServiceCollection AddMyYazd(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MyYazdOptions>(
            configuration.GetSection(MyYazdOptions.Name));

        services.AddHttpClient<IMyYazdService, MyYazdService>(
            client =>
            {
                client.BaseAddress = new Uri(configuration.GetSection(MyYazdOptions.Name).GetValue<string>("BaseAddress") ??
                    throw new Exception("My yazd base url not specified."));
            });

        return services;
    }
}
