using EmitterPersonalAccount.Application.Infrastructure.Cqs;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace EmitterPersonalAccount.Application.Features.Documents
{
    public record DocumentInfoResponse(
        Guid Id,
        string Title,
        string FileExtnsion,
        DateTime UploadDate,
        double Size,
        bool IsEmitterSended)
    { }
    public sealed class GetDocumentsInfoQuery : Query<List<DocumentInfoResponse>>
    {
        public Guid EmitterId { get; set; }
    }

    public sealed class GetDocumentsInfoQueryHandler 
        : QueryHandler<GetDocumentsInfoQuery, List<DocumentInfoResponse>>
    {
        private readonly IRpcClient rpcClient;

        public GetDocumentsInfoQueryHandler(IRpcClient rpcClient)
        {
            this.rpcClient = rpcClient;
        }
        public override async Task<Result<List<DocumentInfoResponse>>> Handle
            (GetDocumentsInfoQuery request, CancellationToken cancellationToken)
        {
            var message = JsonSerializer.Serialize(request);

            var result = await rpcClient
                .CallAsync<List<DocumentInfoResponse>> 
                    (message, RabbitMqAction.GetDocumentInfo, cancellationToken);

            if (!result.IsSuccessfull)
                return Error(new GettingDocumentsInfoError());

            return Success(result.Value);
        }
    }

    public class GettingDocumentsInfoError : Error
    {
        public override string Type => nameof(GettingDocumentsInfoError);
    }
}
