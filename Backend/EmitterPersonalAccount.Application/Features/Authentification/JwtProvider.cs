using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Authentification
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        // В свойстве Value хранится объект, который передаётся через generic
        // <JwtOpions>
        private readonly JwtOptions options = options.Value;
        public string GenerateToken(string userId, Role role)
        {
            var signingCredentials = new SigningCredentials(
                // Так получать секретный ключ небезопасно, потом поправим это
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                SecurityAlgorithms.HmacSha256
                );

            Claim[] claims =
            [
                new (CustomClaims.UserId, userId),
                new (CustomClaims.Role, role.ToString())
            ];

            // claims - словарь ключ - значение, куда мы можем поместить дополнительную информацию о пользователе
            // например, его idшник, имя
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(options.ExpiresHours)
            );

            // Этот хэндлер записывает токен в строку
            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
