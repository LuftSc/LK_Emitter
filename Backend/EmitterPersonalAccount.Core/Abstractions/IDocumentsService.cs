using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IDocumentsService
    {
        Task<Result> SendToRecipientAsync(SendDocumentEvent sendDocumentEvent, CancellationToken cancellationToken);
    }
}