using Microsoft.EntityFrameworkCore;
using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Api.Middlewares;
using Serilog;
using Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
.AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddApi(builder.Configuration);


//Logging
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


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

//logging http requests
app.UseSerilogRequestLogging();

builder.WebHost.UseUrls("http://0.0.0.0:80", "https://0.0.0.0:443");
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
//app.UseAccessControlMiddleware();
app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.MapControllers();

app.Run();

