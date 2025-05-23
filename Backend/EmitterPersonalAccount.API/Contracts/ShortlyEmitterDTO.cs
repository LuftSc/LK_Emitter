using EmitterPersonalAccount.Core.Domain.Models.Postgres.EmitterModel.EmitterVO;

namespace EmitterPersonalAccount.API.Contracts
{
    public record ShortlyEmitterDTO(
        EmitterInfoRecord EmitterInfo,
        int IssuerId
        )
    {
    }
}
