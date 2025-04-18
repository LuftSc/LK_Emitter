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

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition");
                });
            });

            var app = builder.Build();

            // Отключаем HTTPS redirection для разработки
            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.MapHub<ResultsHub>("/results-hub");

            app.Run();
        }
    }
}
