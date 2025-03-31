using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Documents
{
    public sealed class SendDocumentsCommand : Command
    {
        public Guid SenderId { get; set; } = Guid.Empty;
        public Guid EmitterId { get; set; }
        public List<IFormFile> Files { get; set; } = [];
        public bool WithDigitalSignature { get; set; } = false;
    }

    public sealed class SendDocumentsCommandHandler
        : CommandHandler<SendDocumentsCommand>
    {
        private readonly IRabbitMqPublisher publisher;

        public SendDocumentsCommandHandler(IRabbitMqPublisher publisher)
        {
            this.publisher = publisher;
        }
        public override async Task<Result> Handle(SendDocumentsCommand request, 
            CancellationToken cancellationToken)
        {
            Console.WriteLine("Зашли в обработчик команды документов");
            var documents = new List<DocumentInfo>(request.Files.Count);

            foreach (var file in request.Files)
            {
                await using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var fileContent = stream.GetBuffer();

                documents.Add(new DocumentInfo() 
                { 
                    FileName = file.FileName, 
                    ContentType = file.ContentType, 
                    Content = fileContent 
                });
            }

            var sendingEvent = new SendDocumentEvent 
            { 
                SenderId = request.SenderId,
                EmitterId = request.EmitterId,
                Documents = documents,
                WithDigitalSignature = request.WithDigitalSignature
            };

            var message = JsonSerializer.Serialize(sendingEvent);

            var isSuccessfull = await publisher.SendMessageAsync
                (message, RabbitMqAction.SendDocument, cancellationToken);

            if (!isSuccessfull) return Result.Error(new SendingDocumentError());

            return Result.Success();
        }
    }

    public class SendingDocumentError : Error
    {
        public override string Type => nameof(SendingDocumentError);
    }
}
