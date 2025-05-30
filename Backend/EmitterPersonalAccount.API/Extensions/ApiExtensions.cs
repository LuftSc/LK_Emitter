﻿//using AuthService.Authentification;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Application.Services;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EmitterPersonalAccount.API.Extensions
{
    public static class ApiExtensions
    {
        public static void AddApiAuthentification(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        // Нужно ли валидировать издателя?
                        ValidateIssuer = false,
                        // Нужно ли валидировать получателя?
                        ValidateAudience = false,
                        // Нужно ли валидировать время жизни токена?
                        ValidateLifetime = true,
                        // Нужно ли валидировать ключ(secret key) издателя?
                        ValidateIssuerSigningKey = true,
                        // Какой алгоритм нам нужен для кодирования?
                        IssuerSigningKey = new SymmetricSecurityKey(
                            // Достаём секретный ключ из конфигурации
                            Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // Указываем, что токен мы храним в куках
                            context.Token = context.Request.Cookies["tasty-cookies"];

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddScoped<IPermissionService, PermissionService>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddAuthorization();
        }

        public static IEndpointConventionBuilder RequirePermission<TBuilder>(
            this TBuilder builder, params Permission[] permissions)
            where TBuilder : IEndpointConventionBuilder
        {
            return builder.RequireAuthorization(policy =>
            {
                policy.AddRequirements(new PermissionRequirement(permissions));
            });
        }
    }
}
