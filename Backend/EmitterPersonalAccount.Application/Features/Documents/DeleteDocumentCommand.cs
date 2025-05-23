using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Documents
{
    public sealed class DeleteDocumentCommand : Command
    {
        public Guid DocumentId { get; set; }
    }

    public sealed class DeleteDocumentCommandHandler 
        : CommandHandler<DeleteDocumentCommand>
    {
        private readonly IRabbitMqPublisher publisher;

        public DeleteDocumentCommandHandler(IRabbitMqPublisher publisher)
        {
            this.publisher = publisher;
        }
        public override async Task<Result> Handle
            (DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var message = JsonSerializer.Serialize(request.DocumentId);

            var isSuccessfull = await publisher
                .SendMessageAsync(message, RabbitMqAction.DeleteDocument, cancellationToken);

            if (!isSuccessfull)
                return Result.Error(new SendingDeleteCommandError());

            return Result.Success();    
        }
    }

    public class SendingDeleteCommandError : Error
    {
        public override string Type => nameof(SendingDeleteCommandError);
    }

}
