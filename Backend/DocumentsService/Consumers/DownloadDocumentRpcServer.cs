using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace DocumentsService.Consumers
{
    public class DownloadDocumentRpcServer
        : RpcServerBase<Result<DocumentInfo>>
    {
        private readonly IServiceProvider provider;

        public DownloadDocumentRpcServer(string rabbitUri, 
            IServiceProvider provider) : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result<DocumentInfo>> 
            OnMessageProcessingAsync(string message, BasicDeliverEventArgs args)
        {
            var documentId = JsonSerializer
                .Deserialize<Guid>(message);

            Result<Document> serviceResult;

            using (var scope = provider.CreateScope()) 
            {
                var documentService = scope.ServiceProvider
                    .GetRequiredService<IDocumentsService>();

                serviceResult = await documentService
                    .DownloadDocumentById(documentId, default);

                if (serviceResult.IsSuccessfull && 
                    !string.IsNullOrEmpty(serviceResult.Value.Hash))
                // Если документ загружали с ЭЦП
                {
                    var hashService = scope.ServiceProvider
                        .GetRequiredService<IHashService>();

                    var computedHash = hashService
                        .ComputeHash(serviceResult.Value.Content);

                    if (computedHash != serviceResult.Value.Hash)
                    {
                        return Result<DocumentInfo>
                            .Error(new DocumentAuthentificationError()
                            {
                                Data = { {
                                        "Hash verification Error",
                                        "The document was changed by someone."
                                    } }
                            });
                    }
                }
            }

            if (serviceResult.IsSuccessfull)
            {
                
                var documentInfo = new DocumentInfo()
                {
                    Content = serviceResult.Value.Content,
                    ContentType = MIMETypeMapper.GetMimeType(serviceResult.Value.Type),
                    FileName = serviceResult.Value.Title
                };

                return Result<DocumentInfo>.Success(documentInfo);
            }

            return Result<DocumentInfo>.Error(new DownloadDocumentError());
        }

     

        public class DownloadDocumentError : Error
        {
            public override string Type => nameof(DownloadDocumentError);
        }

        public class DocumentAuthentificationError : Error
        {
            public override string Type => nameof(DocumentAuthentificationError);
        }


    }
}
