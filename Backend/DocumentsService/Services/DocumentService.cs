using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;

namespace DocumentsService.Services
{
    /* public class RecipientNotFound : Error
     {
         public override string Type => nameof(RecipientNotFound);
     }
     public class DocumentCreatingError : Error
     {
         public override string Type => nameof(DocumentCreatingError);
     }*/
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
            Console.WriteLine("Попали в сервис документов");
            return await documentRepository
                .AddRangeToUserByEmail(sendDocumentEvent.RecipientEmail,
                sendDocumentEvent.Documents, cancellationToken,
                sendDocumentEvent.WithDigitalSignature);
        }
    }
}
