
using BaseMicroservice;
using EmitterPersonalAccount.API.Extensions;
using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Application.Features.Documents;
//using EmitterPersonalAccount.Application.Hubs;
using EmitterPersonalAccount.Application.Infrastructure.CacheManagment;
using EmitterPersonalAccount.Application.Infrastructure.RabbitMq;
using EmitterPersonalAccount.Application.Infrastructure.Rpc;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Configuration;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Storage;
using EmitterPersonalAccount.DataAccess;
using EmitterPersonalAccount.DataAccess.Repositories;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;

using AuthorizationOptions = EmitterPersonalAccount.Core.Domain.Models.Configuration.AuthorizationOptions;

namespace EmitterPersonalAccount.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // �������������� ��������� ��������������
            builder.Services.AddApiAuthentification(builder.Configuration);
            // ������� ��������� ���������� ���������
            builder.Configuration.AddEnvironmentVariables();

            builder.Services.Configure<RabbitMqInitOptions>
                (builder.Configuration.GetSection(nameof(RabbitMqInitOptions)));

            builder.Services.Configure<JwtOptions>
                (builder.Configuration.GetSection(nameof(JwtOptions)));

            builder.Services.Configure<AuthorizationOptions>
                (builder.Configuration.GetSection(nameof(AuthorizationOptions)));

            builder.Services.AddDbContext<EmitterPersonalAccountDbContext>(options =>
                options.UseNpgsql(builder.Configuration
                    .GetConnectionString(nameof(EmitterPersonalAccountDbContext))
            ));

            /*builder.Services.AddMassTransit(x =>
            {
                x.AddBus(provider =>
                {
                    Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(builder.Configuration.GetConnectionString("RabbitMqUri"));
                        
                        cfg.ReceiveEndpoint("Test", epc =>
                        {
                            epc.ConfigureConsumer<>(provider);
                        });
                    });
                });
            }); */

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

            builder.Services.AddScoped<IOutboxMessagesRepository, OutboxMessagesRepository>();

            builder.Services.AddScoped<IOutboxService, OutboxService>();

            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                options.Configuration = connection;
            });

            builder.Services.AddHostedService<MigrationHostedService>();

            

            builder.Services.AddHostedService<RabbitMqInitializer>();
            builder.Services.AddHostedService<RpcClientInitializer>();

            builder.Services.AddHostedService<OutboxPublisherService>();

            /*builder.Services.AddHostedService<ConsumerRunService>(provider => new ConsumerRunService(
                builder.Configuration,
                provider
                ));*/

            builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();
            

            //builder.Services.AddScoped<ResultService>();

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

            //builder.Services.AddSignalR();

            // 2. ��������� HttpClient ��� ������������� � NotificationService
           /* builder.Services.AddHttpClient("ResultHubService", client =>
            {
                client.BaseAddress = new Uri("http://localhost:5001"); // ��� ������ URL
            });*/

            builder.Services.AddMemoryCache();

            builder.Services.AddHealthChecks();

            var app = builder.Build();

            //app.MapHub<ResultsHubProxy>("/resultsHub");

            app.UseWebSockets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // �������������� ������ ��� ������ ����������
            app.UseCookiePolicy(new CookiePolicyOptions
                {
                // ����� �����-������ js-��� � �������� �� ��� ��������� ���� ����

                    MinimumSameSitePolicy = SameSiteMode.Strict,
                    // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    //MinimumSameSitePolicy = SameSiteMode.None,

                    // ����� �� ����� ���������� ���� ���� ������ �� HTTPS-���������
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
