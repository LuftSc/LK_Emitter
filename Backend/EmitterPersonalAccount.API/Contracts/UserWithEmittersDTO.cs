using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;

namespace EmitterPersonalAccount.API.Contracts
{
    public record UserWithEmittersDTO(
        Guid Id,
        string FullName,
        string Email,
        string Phone,
        DateOnly BirthDate,
        DecryptedPassport Passport,
        Role Role,
        List<EmitterInfoDTO> Emitters
        )
    {
    }
}
