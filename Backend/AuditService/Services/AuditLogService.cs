using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;

namespace AuditService.Services
{
    public class AuditLogService : IDisposable, IAuditLogService
    {
        private readonly IAuditRepository auditRepository;
        private readonly List<UserActionLog> batchBuffer = new();
        private readonly Timer flushTimer;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private readonly int maxBatchSize = 100;
        private readonly TimeSpan flushInterval = TimeSpan.FromSeconds(5);
        public AuditLogService(IAuditRepository auditRepository)
        {
            this.auditRepository = auditRepository;
            flushTimer = new Timer(async _ => await FlushBatchAsync(),
                              null,
                              flushInterval,
                              flushInterval);
        }
        public async Task AddLogAsync(UserActionLog log)
        {
            await _lock.WaitAsync();
            try
            {
                batchBuffer.Add(log);
                if (batchBuffer.Count >= maxBatchSize)
                {
                    await FlushBatchAsync();
                }
            }
            finally
            {
                _lock.Release();
            }
        }
        private async Task FlushBatchAsync(object? state = null)
        {
            List<UserActionLog> logsToSave;
            await _lock.WaitAsync();
            try
            {
                if (batchBuffer.Count == 0) return;

                logsToSave = new List<UserActionLog>(batchBuffer);
                batchBuffer.Clear();
            }
            finally
            {
                _lock.Release();
            }

            try
            {
                await auditRepository.AddRangeAsync(logsToSave, default);
            }
            catch (Exception ex)
            {
                await _lock.WaitAsync();
                try
                {
                    batchBuffer.InsertRange(0, logsToSave);
                }
                finally
                {
                    _lock.Release();
                }
                throw;
            }
        }
        public async void Dispose()
        {
            flushTimer.Dispose();
            await FlushBatchAsync();
        }
    }
}
