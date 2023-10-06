using Api.Configurations;
using Api.Hubs;
using Api.Services.Tools;
using Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shahrbin.Api.Middlewares;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Application;
using Infrastructure;
using Infrastructure.Communications.PushNotification;
using Infrastructure.Persistence;
using Domain.Models.Relational.IdentityAggregate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation    
    swagger.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "ShahrbinMi Web API",
        Description = ""
    });
    // To Enable authorization using Swagger (JWT)    
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
});

//Cors policy
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        b =>
        {
            b
                .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? throw new Exception("Allowd origins cannot be null."))
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .WithExposedHeaders("X-Pagination", "Captcha-Key")
                .AllowAnyHeader()
                .AllowCredentials();
        });
});


//For Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = true;
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddPasswordValidator<CustomPasswordValidator<ApplicationUser>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    //Adding Jwt Bearer
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? throw new Exception("Jwt secret cannot be null.")))
        };
    });

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Configurations
builder.Services.Configure<SmsOptions>(builder.Configuration.GetSection(SmsOptions.Name));
builder.Services.Configure<GeneralSettings>(builder.Configuration.GetSection(GeneralSettings.Name));
builder.Services.Configure<ParsiMapOptions>(builder.Configuration.GetSection(ParsiMapOptions.Name));
builder.Services.Configure<FirebaseProxyOptions>(builder.Configuration.GetSection(FirebaseProxyOptions.Name));
builder.Services.Configure<FeedbackOptions>(builder.Configuration.GetSection(FeedbackOptions.Name));
builder.Services.Configure<List<AppVersion>>(builder.Configuration.GetSection("AppVersions"));
builder.Services.Configure<ImageQualityOptions>(builder.Configuration.GetSection(ImageQualityOptions.Name));

builder.Services.AddSignalR();
// ReCaptcha
//builder.Services.AddOptions<CaptchaSettings>().BindConfiguration("Captcha");
//builder.Services.AddTransient<CaptchaVerificationService>();

//Firebase Cloud Messaging
var generalSettings = builder.Configuration.GetSection(GeneralSettings.Name).Get<GeneralSettings>();
if (generalSettings == null)
    throw new Exception("General settings cannot be null.");
if (generalSettings.UseProxy)
{
    //builder.Services.AddSingleton<IFirebaseCloudMessaging>(new FirebaseCloudMessagingProxy(generalSettings.ProxyUrl));
}
else
{
    builder.Services.AddSingleton<IFirebaseCloudMessaging>(new FirebaseCloudMessaging());
}

//Captcha provider
//builder.Services.AddSingleton<ICaptchaProvider, CaptchaProvider>();


//NotificationHostedService
//builder.Services.AddHostedService<SendNotificationsHostedService>();

//Adding static settings
builder.Services.AddMemoryCache();













var app = builder.Build();

// Perform migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

builder.WebHost.UseUrls("http://0.0.0.0:80", "https://0.0.0.0:443");
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseAccessControlMiddleware();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<NewEventHub>("/eventhub");


app.Run();

