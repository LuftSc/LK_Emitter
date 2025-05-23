using BaseMicroservice;
using EmitterPersonalAccount.Core.Abstractions;
using EmitterPersonalAccount.Core.Domain.SharedKernal.Result;
using RabbitMQ.Client.Events;

namespace DocumentsService.Consumers
{
    public class DeleteDocumentConsumer : BaseRabbitConsumer
    {
        private readonly IServiceProvider provider;

        public DeleteDocumentConsumer(string rabbitUri, IServiceProvider provider)
            :base(rabbitUri)
        {
            this.provider = provider;
        }
        public override async Task<Result> Handler(object model, BasicDeliverEventArgs args)
        {
            var documentId = EventDeserializer<Guid>
                .Deserialize(args);

            Result serviceResult;

            using (var scope = provider.CreateScope()) 
            {
                var documentService = scope.ServiceProvider
                    .GetRequiredService<IDocumentsService>();

                serviceResult = await documentService
                    .DeleteDocumentById(documentId, default);
            }

            return serviceResult;
        }
    }
}
