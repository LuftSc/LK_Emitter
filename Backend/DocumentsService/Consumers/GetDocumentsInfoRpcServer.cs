using BaseMicroservice;
using DocumentsService.Services;
using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System.Collections.Generic;
using System.Text.Json;

namespace DocumentsService.Consumers
{
    public class GetDocumentsInfoRpcServer
        : RpcServerBase<Result<List<DocumentInfoResponse>>>
    {
        private readonly IServiceProvider provider;
        public GetDocumentsInfoRpcServer(string rabbitUri, IServiceProvider provider) 
            : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result<List<DocumentInfoResponse>>>
            OnMessageProcessingAsync(string message)
        {
            var query = JsonSerializer
                .Deserialize<GetDocumentsInfoQuery>(message);

            Result<List<Document>> serviceResult;

            using (var scope = provider.CreateScope())
            {
                var documentsService = scope.ServiceProvider
                    .GetRequiredService<IDocumentsService>();
                serviceResult = await documentsService
                    .GetDocumentsInfoByUserId(query.UserId);
            }
            if (serviceResult.IsSuccessfull)
            {
                var documents = serviceResult.Value
                    .Select(d => new DocumentInfoResponse
                        (d.Id, d.Title, d.Type, d.UploadDate, d.GetSize()))
                    .ToList();

                foreach (var document in documents)
                {
                    Console.WriteLine($"{document.Title}");
                }

                return Result<List<DocumentInfoResponse>>.Success(documents);
            }

            return Result<List<DocumentInfoResponse>>.Error(new GettingDocumentError());
        }

        public override Task<Result<List<DocumentInfoResponse>>> 
            OnMessageProcessingFailureAsync(Exception exception)
        {
            return Task.FromResult
                (Result<List<DocumentInfoResponse>>.Error(new GettingDocumentError()));
        }
    }
}
