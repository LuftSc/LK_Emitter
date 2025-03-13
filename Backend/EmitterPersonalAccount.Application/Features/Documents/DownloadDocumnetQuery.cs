using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
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
    public sealed class DownloadDocumnetQuery : Query<DocumentInfo>
    {
        public Guid DocumentId { get; set; }
    }

    public sealed class DownloadDocumnetQueryHandler
        : QueryHandler<DownloadDocumnetQuery, DocumentInfo>
    {
        private readonly IRpcClient rpcClient;

        public DownloadDocumnetQueryHandler(IRpcClient rpcClient)
        {
            this.rpcClient = rpcClient;
        }
        public override async Task<Result<DocumentInfo>> 
            Handle(DownloadDocumnetQuery request, CancellationToken cancellationToken)
        {
            var message = JsonSerializer.Serialize(request);

            var result = await rpcClient.CallAsync<DocumentInfo>(message,
                RabbitMqAction.DownloadDocument, cancellationToken);

            if (!result.IsSuccessfull)
                return Error(new DownloadingDocumentError());

            return Success(result.Value);
        }
    }

    public class DownloadingDocumentError : Error
    {
        public override string Type => nameof(DownloadingDocumentError);
    }
}
