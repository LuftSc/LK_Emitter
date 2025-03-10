using AuthService.Authentification;
using AuthService.DataAccess;
using AuthService.DataAccess.Repositories;
using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using Microsoft.EntityFrameworkCore;

namespace AuthService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Configuration.AddJsonFile("appsettings.Auth.json", optional: false, reloadOnChange: true);
            // Получаем из файла конфигурации секцию JwtOptions для настройки Jwt-токена
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
            
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(config =>
            {
                // Указываем на маркерный интерфейс, чтобы он зарегистрировал хэндлеры
                config.RegisterServicesFromAssemblyContaining<Program>();
            });

            builder.Services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(AuthDbContext)));
            });

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                options.Configuration = connection;
            });

            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            //builder.Services.AddScoped<ISender, EmailSender>();
            builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();

            builder.Services.RegisterRepository<IUserRepository, UserRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
