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

IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureServices((context, services) =>
        {
            // Add your worker service
            services.AddHostedService<Worker>();

            // Add SignalR support
            services.AddSignalR();

            // Add your repository or other services
            services.AddScoped<IClientConsume, clientConsumeService>();

            // the URL needs to be configured!!!
            services.AddDbContext<minimalApiDb>(o => o.UseSqlite("Data source=C:\\Users\\DELL\\source\\repos\\Webapi\\Webapi\\ClientConsume.db"));

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
                endpoints.MapHub<StreamingHub>("/orderHub");

                // Map the API controllers
                endpoints.MapControllers();
            });
        });
    });

await hostBuilder.RunConsoleAsync();
