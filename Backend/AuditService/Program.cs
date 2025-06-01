using AuditService.Consumers;
using AuditService.DataAccess;
using AuditService.DataAccess.Repositories;
using AuditService.Services;
using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;
using static CSharpFunctionalExtensions.Result;

namespace AuditService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile
                ("appsettings.Audit.json", optional: false, reloadOnChange: true);

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddDbContext<AuditServiceDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(AuditServiceDbContext))),
                contextLifetime: ServiceLifetime.Scoped);

            builder.Services.AddSingleton<IAuditLogService, AuditLogService>();
            builder.Services.RegisterRepository<IAuditRepository, AuditRepository>();
            builder.Services.RegisterRepository<IUsersRepository, UsersRepository>();
            builder.Services.RegisterRepository<IEmittersRepository, EmittersRepository>();

            builder.Services.AddScoped<IExcelService, ExcelService>();
            builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();

            var rabbitUri = builder.Configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");
            builder.Services.AddSingleton<IRpcClient>(_ => new AuditRpcClient(rabbitUri));

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddHostedService<MigrationHostedService>();
            
            builder.Services.AddHostedService<MainService>(provider =>
                new MainService(
                    builder.Configuration, 
                    provider, 
                    provider.GetRequiredService<IRpcClient>()));

            

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.Run();
        }
    }
}
