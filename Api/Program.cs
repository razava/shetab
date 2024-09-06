using Microsoft.EntityFrameworkCore;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Api.Middlewares;
using Serilog;
using Api;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Configure logging with Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Enable detailed EF Core logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
    logging.SetMinimumLevel(LogLevel.Debug);  // Set minimum log level to Debug for detailed logs
});

// Add services to the container.
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddApi(builder.Configuration);

var app = builder.Build();

// Perform migrations and log detailed errors
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Log the connection string for debugging purposes
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var connectionString = db.Database.GetConnectionString();
        logger.LogInformation($"Using Connection String: {connectionString}");

        db.Database.Migrate();
        logger.LogInformation("Database migration completed successfully.");
    }
    catch (SqlException ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database migration");

        // Log detailed SQL error information
        logger.LogError("SQL Error Number: {SqlErrorNumber}", ex.Number);
        logger.LogError("SQL Error Message: {SqlErrorMessage}", ex.Message);
        logger.LogError("SQL Error Class: {SqlErrorClass}", ex.Class);
        logger.LogError("SQL Error Line Number: {SqlErrorLineNumber}", ex.LineNumber);

        // Log inner exceptions
        var innerException = ex.InnerException;
        while (innerException != null)
        {
            logger.LogError("Inner Exception: {InnerExceptionMessage}", innerException.Message);
            innerException = innerException.InnerException;
        }

        // Additionally, write to console for debugging
        Console.WriteLine($"SQL Error Number: {ex.Number}");
        Console.WriteLine($"SQL Error Message: {ex.Message}");
        Console.WriteLine($"SQL Error Class: {ex.Class}");
        Console.WriteLine($"SQL Error Line Number: {ex.LineNumber}");

        innerException = ex.InnerException;
        while (innerException != null)
        {
            Console.WriteLine($"Inner Exception: {innerException.Message}");
            innerException = innerException.InnerException;
        }
    }


}

// Configure Swagger
app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
    {
        swaggerDoc.Servers = new List<OpenApiServer>
        {
            new OpenApiServer
            {
                Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{builder.Configuration.GetSection("Swagger:BasePath").Get<string>()}"
            }
        };
    });
});
app.UseSwaggerUI();

// Enable detailed request logging
app.UseSerilogRequestLogging(); // Serilog middleware for logging HTTP requests

// Configure middleware and HTTP request pipeline
builder.WebHost.UseUrls("http://0.0.0.0:80", "https://0.0.0.0:443");
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.MapControllers();

app.Run();
