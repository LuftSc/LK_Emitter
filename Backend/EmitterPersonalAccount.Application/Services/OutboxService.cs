using EmitterPersonalAccount.Application.Features.Authentification;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Services;

public class OutboxService : IOutboxService
{
    private readonly IOutboxMessagesRepository outboxRepository;

    public OutboxService(IOutboxMessagesRepository outboxReposiotry)
    {
        this.outboxRepository = outboxReposiotry;
    }

    public async Task<Result> CreateAndSaveOutboxMessage
        (OutboxMessageType type, string contentJSON, CancellationToken cancellationToken)
    {
        var outboxMsgCreatingResult = OutboxMessage.Create(type, contentJSON);

        if (!outboxMsgCreatingResult.IsSuccessfull)
            return Result.Error(new OutboxMessageCreatingError());

        var outboxMsgSaveResult = await outboxRepository
                .AddAsync(outboxMsgCreatingResult.Value, cancellationToken);

        if (!outboxMsgSaveResult.IsSuccessfull)
            return Result.Error(new OutboxMessageSavingError());

        return Result.Success();
    }

}

public class OutboxMessageCreatingError : Error
{
    public override string Type => nameof(OutboxMessageCreatingError);
}
public class OutboxMessageSavingError : Error
{
    public override string Type => nameof(OutboxMessageSavingError);
}
