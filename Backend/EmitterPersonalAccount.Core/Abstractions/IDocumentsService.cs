using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
namespace EmitterPersonalAccount.Core.Abstractions
{
    public interface IDocumentsService
    {
        Task<Result<List<Document>>> GetDocumentsInfoByUserId(Guid userId);
        Task<Result> SendToRecipientAsync(SendDocumentEvent sendDocumentEvent, 
            CancellationToken cancellationToken);
        Task<Result<Document>> DownloadDocumentById(Guid documentId,
            CancellationToken cancellationToken);
        Task<Result> DeleteDocumentById(Guid documentId,
            CancellationToken cancellationToken);
    }
}