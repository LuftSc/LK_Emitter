using EmitterPersonalAccount.Core.Domain.Enums;

namespace EmitterPersonalAccount.Core.Domain.SharedKernal.DTO
{
    public record UserClaimsData(
        Guid UserId,
        Role Role
        )
    {
    }
}
