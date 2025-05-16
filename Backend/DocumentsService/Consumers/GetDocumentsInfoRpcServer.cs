using BaseMicroservice;
using DocumentsService.DataAccess.Repositories;
using DocumentsService.Services;
using EmitterPersonalAccount.Application.Features.Documents;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.Models.Postgres;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit;
using EmitterPersonalAccount.Core.Domain.Models.Rabbit.Documents;
using EmitterPersonalAccount.Core.Domain.Repositories;
using EmitterPersonalAccount.Core.Domain.SharedKernal.DTO;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using System.Collections.Generic;
using System.Text.Json;

namespace DocumentsService.Consumers
{
    public class GetDocumentsInfoRpcServer
        : RpcServerBase<Result<DocumentPaginationList>>
    {
        private readonly IServiceProvider provider;
        public GetDocumentsInfoRpcServer(string rabbitUri, IServiceProvider provider) 
            : base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result<DocumentPaginationList>>
            OnMessageProcessingAsync(string message)
        {
            var query = JsonSerializer
                .Deserialize<GetDocumentsInfoQuery>(message);

            Result<DocumentPaginationList> serviceResult;

            using (var scope = provider.CreateScope())
            {
                var documentsRepository = scope.ServiceProvider
                    .GetRequiredService<IDocumentRepository>();
                var result = await documentsRepository
                    .GetByPage(query.IssuerId, query.Page, query.PageSize);

                if (result.IsSuccessfull)
                {
                    var paginationList = new DocumentPaginationList(result.Value.Item1,
                        result.Value.Item2
                            .Select(d => new DocumentDTO(d.Id, d.Title, d.Type, 
                            d.UploadDate, d.GetSize(), d.IsEmitterSended))
                            .ToList());
                    return Result<DocumentPaginationList>.Success(paginationList);
                }
            }

            return Result<DocumentPaginationList>.Error(new GettingDocumentError());
        }

        public override Task<Result<DocumentPaginationList>> 
            OnMessageProcessingFailureAsync(Exception exception)
        {
            return Task.FromResult
                (Result<DocumentPaginationList>.Error(new GettingDocumentError()));
        }
    }
}
