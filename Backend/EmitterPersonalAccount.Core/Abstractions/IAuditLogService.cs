using EmitterPersonalAccount.Core.Domain.Models.Postgres;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IAuditLogService
    {
        Task AddLogAsync(UserActionLog log);
        void Dispose();
    }
}