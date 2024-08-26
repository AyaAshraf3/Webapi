using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using Webapi.theModel;
using Webapi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose() // Log all levels globally
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

// Set Serilog as the logging provider
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddScoped<Irepository, ClientConsumeAPIrpository>();

// This configures the context to use SQLite as the database provider, with the database file named ClientConsume.db.
builder.Services.AddDbContext<APIcontext>(o => o.UseSqlite("Data source=ClientConsume.db"));
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

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

