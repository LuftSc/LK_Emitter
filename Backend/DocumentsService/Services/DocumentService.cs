using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System.Text.Json;

namespace DocumentsService.Services
{
    public class DocumentService : IDocumentsService
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IHashService hashService;
        private readonly IRabbitMqPublisher publisher;

        public DocumentService(IDocumentRepository documentRepository,
            IHashService hashService, IRabbitMqPublisher publisher)
        {
            this.documentRepository = documentRepository;
            this.hashService = hashService;
            this.publisher = publisher;
        }
        public async Task<Result> SendToRecipientAsync(SendDocumentEvent sendDocumentEvent,
            CancellationToken cancellationToken)
        {
            var saveToDbResult =  await documentRepository.CreateDocumentsAsync(sendDocumentEvent.SenderId,
                sendDocumentEvent.IssuerId,sendDocumentEvent.Documents, cancellationToken,
                sendDocumentEvent.WithDigitalSignature);

            if (!saveToDbResult.IsSuccessfull) return saveToDbResult;

            var signalREvent = JsonSerializer.Serialize(new SendResultToClientEvent()
            {
                MethodForResultSending = MethodResultSending.ReceiveDocuments,
                ContentJSON = JsonSerializer.Serialize(new DocumentsContent(
                    saveToDbResult.Value,
                    sendDocumentEvent.SenderId.ToString()
                ))
            });
            Console.WriteLine("Отпраляем результат из документ сервиса");
            var delResult = await publisher
                .SendMessageAsync(signalREvent, RabbitMqAction.SendResultToClient, cancellationToken);
            Console.WriteLine($"Результат был отправлен {delResult}");
            return Result.Success();
        }
        public async Task<Result<List<Document>>> GetDocumentsInfoByUserId(Guid userId)
        {
            return await documentRepository.GetByUserId(userId);    
        }
        public async Task<Result<List<Document>>> GetDocumentsInfoByEmitterId(int issuerId)
        {
            return await documentRepository.GetByEmitterId(issuerId);
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
