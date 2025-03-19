using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthService.Authentification
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        // В свойстве Value хранится объект, который передаётся через generic
        // <JwtOpions>
        private readonly JwtOptions options = options.Value;
        public string GenerateToken(string userId)
        {
            var signingCredentials = new SigningCredentials(
                // Так получать секретный ключ небезопасно, потом поправим это
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                SecurityAlgorithms.HmacSha256
                );

            Claim[] claims =
            [
                new (CustomClaims.UserId, userId)
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

