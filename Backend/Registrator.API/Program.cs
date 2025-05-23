
using Registrator.API.Extensions;
using Registrator.DataAccess.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Registrator.API.Services;
using Registrator.DataAccess;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Fonts;

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

            builder.Configuration.AddEnvironmentVariables();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<RegistratorDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(RegistratorDbContext)));
            });

            builder.Services.AddHostedService<MigrationHostedService>();
            builder.Services.RegisterRepository<IDirectivesRepository, DirectiveRepository>();

            GlobalFontSettings.FontResolver = new PDFSharpBuiltinResolver();

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

            app.Run();
        }
    }
}
