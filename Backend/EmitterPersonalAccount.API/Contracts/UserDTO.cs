using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;

namespace EmitterPersonalAccount.API.Contracts
{
    public record UserDTO(
        Guid Id,
        string Email,
        List<EmitterInfo> Emitters
        )
    {
    }
}
