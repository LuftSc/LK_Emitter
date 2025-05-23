using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IJwtProvider
    {
        string GenerateToken(string userId, Role role);
    }
}