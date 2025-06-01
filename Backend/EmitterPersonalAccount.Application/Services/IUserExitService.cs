using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace EmitterPersonalAccount.Application.Services
{
    public interface IUserExitService
    {
        Result OnLogout(Guid userId, CancellationToken cancellationToken = default);
        Result OnReload(Guid userId, CancellationToken cancellationToken = default);
    }
}