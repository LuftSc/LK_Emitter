
using DocumentsService.DataAccess;
using DocumentsService.DataAccess.Repositories;
using DocumentsService.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;

namespace DocumentsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("appsettings.Documents.json", optional: false, reloadOnChange: true);

            builder.Configuration.AddEnvironmentVariables();
            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddDbContext<DocumentsDbContext>();

            builder.Services.AddDbContext<DocumentsDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(DocumentsDbContext)));
            });

            builder.Services.AddScoped<IHashService, HashService>();

            builder.Services.RegisterRepository<IDocumentRepository, DocumentsRepository>();
            builder.Services.AddScoped<IDocumentsService, DocumentService>();

            /*var rabbitUri = builder.Configuration.GetConnectionString("RabbitMqUri");
            ArgumentNullException.ThrowIfNull(rabbitUri, "Rabbit URI can not be null!");*/

            /*var queueName = builder.Configuration.GetConnectionString("RabbitMqQueueName");
            ArgumentNullException.ThrowIfNull(queueName, "Rabbit Queue name can not be null!");*/

            builder.Services.AddHostedService<MigrationHostedService>();

            builder.Services.AddHostedService<MainService>(provider => new MainService(
                builder.Configuration,
                provider
            ));

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
