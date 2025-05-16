using EmitterPersonalAccount.Core.Domain.Enums;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IPermissionService
    {
        Task<HashSet<Permission>> GetPermissionAsync(Guid userId);
    }
}