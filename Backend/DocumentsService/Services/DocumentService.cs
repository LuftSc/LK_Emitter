using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace DocumentsService.Services
{
    public class DocumentService : IDocumentsService
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IHashService hashService;

        public DocumentService(IDocumentRepository documentRepository,
            IHashService hashService)
        {
            this.documentRepository = documentRepository;
            this.hashService = hashService;
        }
        public async Task<Result> SendToRecipientAsync(SendDocumentEvent sendDocumentEvent,
            CancellationToken cancellationToken)
        {
            return await documentRepository.AddRangeByEmitterId(sendDocumentEvent.SenderId,
                sendDocumentEvent.EmitterId,sendDocumentEvent.Documents, cancellationToken,
                sendDocumentEvent.WithDigitalSignature);
        }
        public async Task<Result<List<Document>>> GetDocumentsInfoByUserId(Guid userId)
        {
            return await documentRepository.GetByUserId(userId);    
        }
        public async Task<Result<List<Document>>> GetDocumentsInfoByEmitterId(Guid emitterId)
        {
            return await documentRepository.GetByEmitterId(emitterId);
        }
        public async Task<Result<Document>> DownloadDocumentById(Guid documentId, 
            CancellationToken cancellationToken)
        {
            var document = await documentRepository
                .FindAsync([documentId], cancellationToken);

            if (document is null)
                return Result<Document>.Error(new GettingDocumentError());

            return Result<Document>.Success(document);
        }
        public async Task<Result> DeleteDocumentById(Guid documentId,  
            CancellationToken cancellationToken)
        {
            return await documentRepository
                .DeleteByIdAsync(documentId, cancellationToken);
        }
    }
    public class GettingDocumentError : Error
    {
        public override string Type => nameof(GettingDocumentError);
    }
}
