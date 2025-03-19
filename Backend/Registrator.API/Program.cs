
using Registrator.API.Extensions;
using Registrator.DataAccess.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Registrator.API.Services;
using Registrator.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Registrator.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile("appsettings.Registrator.json", optional: false, reloadOnChange: true);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<RegistratorDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(RegistratorDbContext)));
            });

            builder.Services.RegisterRepository<IDirectivesRepository, DirectiveRepository>();

            builder.Services.AddScoped<PdfGeneratorService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.AddMappedEndpoints();
/*
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            
            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();*/

            app.Run();
        }
    }
}
