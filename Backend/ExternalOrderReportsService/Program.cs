
using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using ExternalOrderReportService.DataAccess;
using ExternalOrderReportService.DataAccess.Repositories;
using ExternalOrderReportsService.Publishers;
using ExternalOrderReportsService.Services;
using Microsoft.EntityFrameworkCore;

namespace ExternalOrderReportsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.Reports.json", optional: false, reloadOnChange: true);
            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Configuration.AddEnvironmentVariables();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient();

            builder.Services.RegisterRepository<IOrderReportsRepository, OrderReportsRepository>();

            builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();
            builder.Services.AddScoped<IOrderReportsService, OrderReportsService>();
            builder.Services.AddScoped<IReportStatusChangeService, ReportStatusChangeService>();

            builder.Services.AddHostedService<MainService>(provider => new MainService(
                builder.Configuration,
                provider
            ));

            builder.Services.AddDbContext<ExternalOrderReportServiceDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration
                    .GetConnectionString(nameof(ExternalOrderReportServiceDbContext)));
            });

            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.Run();
        }
    }
}
