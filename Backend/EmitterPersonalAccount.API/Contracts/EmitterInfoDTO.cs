using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;

namespace EmitterPersonalAccount.API.Contracts
{
    public record EmitterInfoDTO(
        Guid Id,
        EmitterInfo EmitterInfo
        )
    {
        
    }
}
