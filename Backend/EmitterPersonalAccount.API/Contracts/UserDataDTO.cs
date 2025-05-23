using EmitterPersonalAccount.Core.Domain.Enums;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;

namespace EmitterPersonalAccount.API.Contracts
{
    public record UserDataDTO(
        Guid Id,
        string Name,
        string Surname,
        string Patronymic,
        string Email,
        string Phone,
        DateOnly BirthDate,
        DecryptedPassport Passport,
        Role Role
        )
    {
    }
}
