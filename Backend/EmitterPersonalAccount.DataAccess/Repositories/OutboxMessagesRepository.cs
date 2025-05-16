using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.DataAccess.Repositories
{
    public class OutboxMessagesRepository : IOutboxMessagesRepository
    {
        private readonly EmitterPersonalAccountDbContext context;

        public OutboxMessagesRepository(EmitterPersonalAccountDbContext context)
        {
            this.context = context;
        }
        public async Task<Result<List<OutboxMessage>>> GetNewlyAdded(CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();

            var messages = await context.OutboxMessages
                .AsNoTracking()
                .Where(m => !m.IsMessageHasBeenProcessed)
                .OrderBy(m => m.Timestamp)
                .Take(20)
                .ToListAsync(cancellation);

            return Result<List<OutboxMessage>>.Success(messages);
        }

        public async Task<Result> SetStatusProcessed(Guid id, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();

            await context.OutboxMessages
                .Where(m => m.Id == id)
                .ExecuteUpdateAsync(m => m
                    .SetProperty(p => p.IsMessageHasBeenProcessed, true)
                    .SetProperty(p => p.Error, string.Empty), cancellation);

            return Result.Success();
        }

        public async Task<Result> SetStatusFailed(Guid id, string error, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();

            await context.OutboxMessages
                .Where(m => m.Id == id)
                .ExecuteUpdateAsync(m => m
                    .SetProperty(p => p.Error, error), cancellation);

            return Result.Success();
        }
        public async Task<Result> AddAsync(OutboxMessage message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await context.OutboxMessages.AddAsync(message, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
