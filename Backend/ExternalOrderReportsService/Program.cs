
using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using ExternalOrderReportService.DataAccess;
using ExternalOrderReportService.DataAccess.Repositories;
using ExternalOrderReportsService.Configurations;
using ExternalOrderReportsService.Publishers;
using ExternalOrderReportsService.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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

            builder.Services.Configure<RegistratorEndpointOptions>
                (builder.Configuration.GetSection(nameof(RegistratorEndpointOptions)));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient();

            builder.Services.RegisterRepository<IOrderReportsRepository, OrderReportsRepository>();

            builder.Services.AddScoped<IRequestSender, RequestSender>();
            builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();
            builder.Services.AddScoped<IOrderReportsService, OrderReportsService>();
            builder.Services.AddScoped<IReportStatusChangeService, ReportStatusChangeService>();

            builder.Services.AddHostedService<MainService>(provider => new MainService(
                builder.Configuration,
                provider
            ));

            builder.Services.AddDbContext<ExternalOrderReportServiceDbContext>(options =>
            {
                var connectionString = builder.Configuration
                    .GetConnectionString(nameof(ExternalOrderReportServiceDbContext));

                var npgsqlBuilder = new NpgsqlDataSourceBuilder(connectionString);

                npgsqlBuilder.EnableDynamicJson();

                options.UseNpgsql(npgsqlBuilder.Build());
            });

            builder.Services.AddHostedService<MigrationHostedService>();

            
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
