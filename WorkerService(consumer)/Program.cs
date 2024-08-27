using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using streamer;
using Microsoft.AspNetCore.Builder;
using streamer.Hubs;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.EntityFrameworkCore;
using streamer.minimalAPI;
using streamer.theModel;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {

        webBuilder.ConfigureAppConfiguration((context, config) =>
          {
              // Load the configuration from appsettings.json
              config.SetBasePath(Directory.GetCurrentDirectory());
              config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
          });
        
        webBuilder.ConfigureServices((context, services) =>
        {
            var configuration = context.Configuration;

            // Add your worker service
            services.AddHostedService<Worker>();

            // Add SignalR support
            services.AddSignalR();

            // Add your repository or other services
            services.AddScoped<IClientConsume, clientConsumeService>();


            // Register IDistributedCache with Redis implementation
            services.AddDistributedRedisCache(
               options =>
               {
                   options.Configuration = configuration.GetSection("Redis")["ConnectionString"];
               });

            // Add Controllers
            services.AddControllers();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkerService API", Version = "v1" });
            });
        });

        webBuilder.Configure((context, app) =>
        {
            var configuration = context.Configuration;
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WorkerService API v1");
                c.RoutePrefix = string.Empty; // To serve the Swagger UI at the app's root
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Map the SignalR Hub endpoint
                endpoints.MapHub<StreamingHub>(configuration["AppSettings:SignalrEndpoint"]);

                // Map the API controllers
                endpoints.MapControllers();
            });
        });
    });

await hostBuilder.RunConsoleAsync();
