
using BaseMicroservice;
using EmitterPersonalAccount.API.Extensions;
using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.Application.Infrastructure.RabbitMq;
using EmitterPersonalAccount.Application.Infrastructure.Rpc;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using MediatR;

namespace EmitterPersonalAccount.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Устанаваливаем настройки аутентификации
            builder.Services.AddApiAuthentification(builder.Configuration);

            builder.Services.Configure<RabbitMqInitOptions>
                (builder.Configuration.GetSection(nameof(RabbitMqInitOptions)));

            builder.Services.AddHostedService<RabbitMqInitializer>();

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

            builder.Services.AddHttpClient();

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                options.Configuration = connection;
            });
            
            builder.Services.AddSingleton<IRpcClient, RpcClient>();
            builder.Services.AddHostedService<RpcClientInitializer>();

            builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();

            var app = builder.Build();

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
                    //MinimumSameSitePolicy = SameSiteMode.None,

                    // Чтобы мы могли отправлять наши куки ТОЛЬКО по HTTPS-протоколу
                    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,

                    Secure = CookieSecurePolicy.Always
                    //Secure = CookieSecurePolicy.SameAsRequest
                }

            );

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
