
using BaseMicroservice;
using EmitterPersonalAccount.API.Extensions;
using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.Application.Hubs;
using EmitterPersonalAccount.Application.Infrastructure.CacheManagment;
using EmitterPersonalAccount.Application.Infrastructure.RabbitMq;
using EmitterPersonalAccount.Application.Infrastructure.Rpc;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using EmitterPersonalAccount.DataAccess;
using EmitterPersonalAccount.DataAccess.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmitterPersonalAccount.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Устанаваливаем настройки аутентификации
            builder.Services.AddApiAuthentification(builder.Configuration);
            // Сначала загружаем переменные окружения
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.Configure<RabbitMqInitOptions>
                (builder.Configuration.GetSection(nameof(RabbitMqInitOptions)));

            builder.Services.Configure<JwtOptions>
                (builder.Configuration.GetSection(nameof(JwtOptions)));

            builder.Services.AddDbContext<EmitterPersonalAccountDbContext>(options =>
                options.UseNpgsql(builder.Configuration
                    .GetConnectionString(nameof(EmitterPersonalAccountDbContext))
            ));

            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            builder.Services.AddScoped<IMemoryCacheService, MemoryCacheService>();

            //builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<IRpcClient, RpcClient>();
            

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddMediatR(x =>
            {
                //x.RegisterServicesFromAssemblyContaining<Program>();
                x.RegisterServicesFromAssembly(typeof(SendDocumentsCommandHandler).Assembly);
            });

            //builder.Services.AddHttpClient();

            builder.Services.RegisterRepository<IUserRepository, UsersRepository>();
            builder.Services.RegisterRepository<IEmittersRepository, EmittersRepository>();
            builder.Services.RegisterRepository<IRegistratorRepository, RegistratorRepository>();
            builder.Services.RegisterRepository<IOrderReportsRepository, OrderReportsRepository>();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                options.Configuration = connection;
            });

            builder.Services.AddHostedService<MigrationHostedService>();

            builder.Services.AddHostedService<RabbitMqInitializer>();
            builder.Services.AddHostedService<RpcClientInitializer>();

            builder.Services.AddHostedService<ConsumerRunService>(provider => new ConsumerRunService(
                builder.Configuration,
                provider
                ));

            builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();
            

            builder.Services.AddScoped<ResultService>();

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

            builder.Services.AddSignalR();

            builder.Services.AddMemoryCache();

            builder.Services.AddHealthChecks();

            var app = builder.Build();

            app.MapHub<ResultsHub>("/resultsHub");

            app.UseWebSockets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Дополнительная защита для нашего приложения
            app.UseCookiePolicy(new CookiePolicyOptions
                {
                // Чтобы какой-нибудь js-код в браузере не мог прочитать наши куки

                    MinimumSameSitePolicy = SameSiteMode.Strict,
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    //MinimumSameSitePolicy = SameSiteMode.None,

                    // Чтобы мы могли отправлять наши куки ТОЛЬКО по HTTPS-протоколу
                    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,

                    Secure = CookieSecurePolicy.Always
                    //Secure = CookieSecurePolicy.SameAsRequest
                }

            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x =>
            {
                x.WithHeaders().AllowAnyHeader();
                x.WithOrigins("http://localhost:3000");
                x.WithMethods().AllowAnyMethod();
                x.WithOrigins("http://localhost:3000").AllowCredentials();
                x.WithExposedHeaders("Content-Disposition");
            });

            app.MapHealthChecks("/health");

            app.MapControllers();
            app.Run();
        }
    }
}
