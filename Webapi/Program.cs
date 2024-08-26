using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using Webapi.theModel;
using Webapi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.Extensions.Configuration;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from appsettings.json
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Read settings from configuration
    .Enrich.FromLogContext()
    .CreateLogger();

// Set Serilog as the logging provider
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddScoped<Irepository, ClientConsumeAPIrpository>();

// Use the configuration from appsettings.json,This configures the context to use SQLite as the database provider, with the database file named ClientConsume.db.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<APIcontext>(o => o.UseSqlite(connectionString));

// Register the OrderSender service
builder.Services.AddScoped<OrderSender>();

builder.Services.AddControllers();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Use Serilog request logging
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});



try
{
    Log.Information("Starting up the application!!!");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed!!!!");
}
finally
{
    Log.CloseAndFlush();
}

