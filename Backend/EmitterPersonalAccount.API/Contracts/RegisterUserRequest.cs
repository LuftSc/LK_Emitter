using EmitterPersonalAccount.Core.Domain.Enums;

namespace EmitterPersonalAccount.API.Contracts
{
    public record RegisterUserRequest(
        string Email,
        string Password,
        List<Guid>? EmittersGuids,
        DateOnly? BirthDate,
        PassportDTO? Passport,
        Role Role = Role.User,
        string FullName = "",
        string Phone = ""
        )
    {
    }
}
