using EmitterPersonalAccount.Core.Abstractions;
using ResultHubService.Hubs;
using ResultHubService.Services;

namespace ResultHubService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Configuration.AddJsonFile("appsettings.Results.json", optional: false, reloadOnChange: true);
            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddSignalR();
            builder.Services.AddCors();

            builder.Services.AddMemoryCache();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                options.Configuration = connection;
            });

            builder.Services.AddScoped<IMemoryCacheService, MemoryCacheService>();
            builder.Services.AddScoped<ResultService>();

            builder.Services.AddHostedService<MainService>(provider => 
                new MainService(builder.Configuration, provider));

            var app = builder.Build();

            app.UseCors(b =>
            {
                b.AllowAnyMethod();
                b.AllowAnyHeader();
                b.AllowCredentials();
                b.WithOrigins("http://localhost:5000");
            });
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapHub<ResultsHub>("/results-hub");

            app.Run();
        }
    }
}
